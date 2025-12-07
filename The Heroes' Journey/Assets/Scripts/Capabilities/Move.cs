using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

namespace TheHeroesJourney
{
    [RequireComponent(typeof(Controller))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float maxSpeed;
        [SerializeField, Range(0f, 500f)] private float maxAcceleration;
        [SerializeField, Range(0f, 100f)] private float maxAirAcceleration;
        [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime;

        private float horizontalInput;
        public Vector3 direction;
        private Vector3 desiredVelocity;
        private Vector3 velocity;

        private Controller controller;
        private Rigidbody2D body;
        private CollisionDataRetriever collisionDataRetriever;
        private WallInteractor wallInteractor;

        [Header("Camera")]
        [SerializeField] private GameObject _cameraFollowGO;
        private CameraFollowObject _cameraFollowObject;

        private float maxSpeedChange;
        private float acceleration;
        private float wallStickCounter;
        private bool onGround;
        public bool isFacingRight;

        public Player player;


        private float changeSpeed = 3;
        private bool isMoveLeftButtonHeldDown;
        private bool isMoveRightButtonHeldDown;
        private bool inputByKeyboard = false;

        private void Awake()
        {
            player = GetComponent<Player>();
            inputByKeyboard = player.inputByKeyboard;

            //BAN PHIM
            controller = GetComponent<Controller>();

            collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            body = GetComponent<Rigidbody2D>();
            wallInteractor = GetComponent<WallInteractor>();
            isFacingRight = true;
            _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (PauseManager.isPause || player.isDogHealing)
            {
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                return;
            }

            if (inputByKeyboard)
            {
                //BAN PHIM
                direction.x = controller.input.RetrieveMoveInput();
            }
            else
            {
                //BUTTON
                if (isMoveLeftButtonHeldDown)
                {
                    direction.x -= Time.deltaTime * changeSpeed;
                    if (direction.x < -1)
                        direction.x = -1;
                }
                else if (isMoveRightButtonHeldDown)
                {
                    direction.x += Time.deltaTime * changeSpeed;
                    if (direction.x > 1)
                        direction.x = 1;
                }
                else
                    direction.x = 0;
            }

            desiredVelocity = new Vector3(direction.x, 0f, 0f) * Mathf.Max(maxSpeed - collisionDataRetriever.Friction, 0f);
            player.GetComponentInChildren<Animator>().SetFloat("xVelocity", Mathf.Abs(desiredVelocity.x));
        }

        private void FixedUpdate()
        {
            if (player.isDogHealing)
            {
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                return;
            }

            onGround = collisionDataRetriever.OnGround;
            velocity = body.linearVelocity;


            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

            #region Wall Stick

            if (inputByKeyboard)
            {
                //BAN PHIM
                if (collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && !wallInteractor.isWallJumping)
                {
                    if (wallStickCounter > 0)
                    {
                        velocity.x = 0;

                        if (controller.input.RetrieveMoveInput() == collisionDataRetriever.ContactNormal.x)
                        {
                            wallStickCounter -= Time.deltaTime;
                        }
                        else
                        {
                            wallStickCounter = wallStickTime;
                        }
                    }
                    else
                    {
                        wallStickCounter = wallStickTime;
                    }
                }
            }
            else
            {
                //BUTTON
                if (collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && !wallInteractor.isWallJumping)
                {
                    if (wallStickCounter > 0)
                    {
                        velocity.x = 0;

                        if (direction.x == collisionDataRetriever.ContactNormal.x)
                        {
                            wallStickCounter -= Time.deltaTime;
                        }
                        else
                        {
                            wallStickCounter = wallStickTime;
                        }
                    }
                    else
                    {
                        wallStickCounter = wallStickTime;
                    }
                }
            }

            #endregion

            body.linearVelocity = velocity;

            if (direction.x > 0 || direction.x < 0)
                TurnCheck();
        }
        public void SetDefaultValueMoveButtons()
        {
            isMoveLeftButtonHeldDown = false;
            isMoveRightButtonHeldDown = false;
        }

        public void PointerDownLeft()
        {
            isMoveLeftButtonHeldDown = true;
        }

        public void PointerDownRight()
        {
            isMoveRightButtonHeldDown = true;
        }

        public void PointerUpLeft()
        {
            isMoveLeftButtonHeldDown = false;
        }

        public void PointerUpRight()
        {
            isMoveRightButtonHeldDown = false;
        }


        private void TurnCheck()
        {
            if (direction.x > 0 && !isFacingRight)
                Turn();
            else if (direction.x < 0 && isFacingRight)
                Turn();
        }

        public void Turn()
        {
            if (isFacingRight)
            {
                Vector2 rotator = new Vector2(body.transform.rotation.x, 180f);
                body.transform.rotation = Quaternion.Euler(rotator);
                isFacingRight = !isFacingRight;
                _cameraFollowObject.CallTurn();
            }
            else
            {
                Vector2 rotator = new Vector2(body.transform.rotation.x, 0f);
                body.transform.rotation = Quaternion.Euler(rotator);
                isFacingRight = !isFacingRight;
                _cameraFollowObject.CallTurn();
            }
        }
    }
}

