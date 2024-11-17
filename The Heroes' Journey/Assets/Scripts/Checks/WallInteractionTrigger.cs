using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class WallInteractionTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && TutorialManager.Instance != null && collision.CompareTag("Player"))
            {
                if(TutorialManager.Instance.GetCurrentPopUpIndex() == 13)
                    TutorialManager.Instance.slideWall = true;
                else if(TutorialManager.Instance.GetCurrentPopUpIndex() == 14)
                    TutorialManager.Instance.popUps[14].SetActive(true);
            }
        }
    }
}

