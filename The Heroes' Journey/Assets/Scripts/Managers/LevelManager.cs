using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheHeroesJourney
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;
        public GameObject loadingScreen;
        public Slider slider;
        public TMP_Text progressText;


        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LevelManager>();
                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
                Destroy(this.gameObject);
        }


        public void LoadLevel(int levelIndex)
        {
            StartCoroutine(LoadAsynchronously(levelIndex));
        }

        public void ReturnToMainMenu()
        {
            if (PauseManager.isPause)
                PauseManager.Instance.TogglePause();

            AudioManager.Instance.Stop("MainGameTheme");
            AudioManager.Instance.Stop("BattleCultisPriestTheme");
            AudioManager.Instance.Stop("BattleJumpKingTheme");

            AudioManager.Instance.Play("BackToMainMenuButton");

            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex - 1));
        }


        IEnumerator LoadAsynchronously(int levelIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
            
            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                slider.value = progress;
                progressText.text = progress * 100 + "%";

                yield return null;
                if (operation.isDone)
                    TransitionManager.Instance.StartCirleWipeTrasition();
            }
        }
    }
}

