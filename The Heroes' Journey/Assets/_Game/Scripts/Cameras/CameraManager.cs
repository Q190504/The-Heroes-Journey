using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

namespace TheHeroesJourney
{
    public class CameraManager : MonoBehaviour, IDataPresistence
    {
        public static CameraManager _instance;

        [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

        [Header("Controls for lerping the Y Damping during player jum/fall")]
        [SerializeField] private float _fallPanAmount = 0.5f;
        [SerializeField] private float _fallPanTime = 0.35f;
        public float _fallSpeedYDampingChangeThreshold = -15f;

        public bool IsLerpingYDamping { get; private set; }

        public bool LerpedFromPlayerFalling { get; set; }

        private Coroutine _lerpYPanCoroutine;
        private Coroutine _panCameraCoroutine;

        private string currentVirtualCameraName;
        private string savedVirtualCameraName;
        private CinemachineVirtualCamera _currentCamera;
        private CinemachineFramingTransposer _framingTransposer;

        private float _normYPanAmount;

        private Vector2 _startingTrackedObjectOffset;

        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<CameraManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

        }

        private void Start()
        {
            SetCamera();
        }

        #region Lerp the Y Damping

        public void LerpYDamping(bool isPlayerFalling)
        {
            _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
        }

        private IEnumerator LerpYAction(bool isPlayerFalling)
        {
            IsLerpingYDamping = true;

            //grab the starting damping amount
            float startDampAmount = _framingTransposer.m_YDamping;
            float endDampAmount = 0f;

            //determine the end damping amount
            if (isPlayerFalling)
            {
                endDampAmount = _fallPanAmount;
                LerpedFromPlayerFalling = true;
            }
            else
                endDampAmount = _normYPanAmount;

            //lerp the pan amount
            float elapsedTime = 0f;
            while (elapsedTime < _fallPanTime)
            {
                elapsedTime += Time.deltaTime;

                float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallPanTime);
                _framingTransposer.m_YDamping = lerpedPanAmount;
                yield return null;
            }

            IsLerpingYDamping = false;
        }

        #endregion

        #region Pan camera

        public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
        {
            _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
        }

        private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
        {
            Vector2 endPos = Vector2.zero;
            Vector2 startingPos = Vector2.zero;

            //handle pan from trigger
            if (!panToStartingPos)
            {
                //set the direction and distance
                switch (panDirection)
                {
                    case PanDirection.Up:
                        endPos = Vector2.up;
                        break;
                    case PanDirection.Down:
                        endPos = Vector2.down;
                        break;
                    case PanDirection.Left:
                        endPos = Vector2.left;
                        break;
                    case PanDirection.Right:
                        endPos = Vector2.right;
                        break;
                    default:
                        break;
                }

                endPos *= panDistance;

                startingPos = _startingTrackedObjectOffset;

                endPos += startingPos;
            }

            //handle the direction settings when moving back to the starting position
            else
            {
                startingPos = _framingTransposer.m_TrackedObjectOffset;
                endPos = _startingTrackedObjectOffset;
            }

            //handle the actual panning of the camera
            float elapsedTime = 0f;
            while (elapsedTime < panTime)
            {
                elapsedTime += Time.deltaTime;

                Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / panTime);
                _framingTransposer.m_TrackedObjectOffset = panLerp;

                yield return null;
            }
        }

        #endregion

        #region Swap Camera

        public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
        {
            //if the current camera is the camera on the left and out trigger exit direction was on the right
            if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
            {
                //activate the new camera
                cameraFromRight.enabled = true;
                currentVirtualCameraName = cameraFromRight.gameObject.name;

                //disactivate the old camera
                cameraFromLeft.enabled = false;

                //set the new camera as the current camera
                _currentCamera = cameraFromRight;

                //update our composer variable
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }

            //if the current camera is the camera on the right and out trigger exit direction was on the left
            if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
            {
                //activate the new camera
                cameraFromLeft.enabled = true;
                currentVirtualCameraName = cameraFromLeft.gameObject.name;

                //disactivate the old camera
                cameraFromRight.enabled = false;

                //set the new camera as the current camera
                _currentCamera = cameraFromLeft;

                //update our composer variable
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        #endregion

        public void SetCamera()
        {
            if(savedVirtualCameraName == null)
            {
                for (int i = 0; i < _allVirtualCameras.Length; i++)
                {
                    if (_allVirtualCameras[i].TryGetComponent(out CinemachineVirtualCamera virtualCamera)
                        && virtualCamera.enabled)
                    {
                        //set the current avtive camera
                        _currentCamera = _allVirtualCameras[i];

                        //set the framing transposer
                        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                    }
                    else
                    {
                        _allVirtualCameras[i].enabled = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _allVirtualCameras.Length; i++)
                {
                    if (_allVirtualCameras[i].gameObject.name == savedVirtualCameraName)
                    {
                        _allVirtualCameras[i].enabled = true;

                        //set the current avtive camera
                        _currentCamera = _allVirtualCameras[i];

                        //set the framing transposer
                        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                    }
                    else
                    {
                        _allVirtualCameras[i].enabled = false;
                    }
                }

            }

            //set the YDamping amount so it's based on the inspector value
            _normYPanAmount = _framingTransposer.m_YDamping;

            //set the starting position of the tracked object offset
            _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
        }

        public void LoadData(GameData data)
        {
            currentVirtualCameraName = data.virtualCameraName;
            savedVirtualCameraName = data.virtualCameraName;
        }

        public void SaveData(GameData data)
        {
            savedVirtualCameraName = currentVirtualCameraName;
            data.virtualCameraName = savedVirtualCameraName;
        }
    }

}


