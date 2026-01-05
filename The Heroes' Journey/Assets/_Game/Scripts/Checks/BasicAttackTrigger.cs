using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheHeroesJourney
{
    public class BasicAttackTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && TutorialManager.Instance != null && TutorialManager.Instance.GetCurrentPopUpIndex() == 8)
                TutorialManager.Instance.popUps[8].SetActive(true);
        }
    }
}

