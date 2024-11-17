using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class OpenMapTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && TutorialManager.Instance != null && TutorialManager.Instance != null && TutorialManager.Instance.GetCurrentPopUpIndex() == 7)
                TutorialManager.Instance.popUps[7].SetActive(true);
        }
    }
}
