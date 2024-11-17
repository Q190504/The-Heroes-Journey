using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    public GameObject showErrorPanel;
    public TMP_Text errorDetail;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        AudioManager.Instance.Stop("MainGameTheme");
        AudioManager.Instance.Play("MainMenuTheme");
    }

    public void StartGame()
    {
        FirebaseManager.LogEvent("Button_Play");
        AudioManager.Instance.Play("PlayButton");
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowErrorPanel(string error)
    {
        InGameManager.Instance.player.GetComponent<Move>().SetDefaultValueMoveButtons();
        InGameManager.Instance.player.GetComponent<Jump>().SetDefaultValueJumpButtons();
        AudioManager.Instance.Play("ErrorPanel");
        showErrorPanel.SetActive(true);
        errorDetail.text = error;
    }

    public void CloseShowErrorPanel()
    {
        AudioManager.Instance.Play("ShortClick");
        showErrorPanel.SetActive(false);
    }
}
