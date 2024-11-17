using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;

    public Animator CircleWipeTransition;
    public Animator crossfadeTransition;
    public float transitionTime;

    public static TransitionManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TransitionManager>();
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


    public void StartCirleWipeTrasition()
    {
        StartCoroutine(CirleWipeTrasition());
    }

    IEnumerator CirleWipeTrasition()
    {
        CircleWipeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        LevelManager.Instance.loadingScreen.SetActive(false);
    }

    public void StartCrossfadeTrasition()
    {
        StartCoroutine(CrossfadeTrasition());
    }

    IEnumerator CrossfadeTrasition()
    {
        crossfadeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
    }
}
