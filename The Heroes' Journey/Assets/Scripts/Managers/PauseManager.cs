using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using UnityEngine.Events;

namespace TheHeroesJourney
{
    public class PauseManager : MonoBehaviour
    {
        private static PauseManager _instance;


        float previousTimeScale = 1;
        public static bool isPause;
        public UnityAction<bool> onChangePause;

        public static PauseManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PauseManager>();
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


        // Start is called before the first frame update
        void Start()
        {
            isPause = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TogglePause()
        {
            if (Time.timeScale > 0)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                AudioManager.Instance.Play("Pause");
                isPause = true;
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = previousTimeScale;
                AudioManager.Instance.Play("Unpause");
                isPause = false;
            }
            onChangePause?.Invoke(isPause);
        }
    }
}

