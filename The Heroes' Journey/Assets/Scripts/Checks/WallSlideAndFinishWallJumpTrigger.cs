using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class WallSlideAndFinishWallJumpTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && TutorialManager.Instance != null && collision.CompareTag("Player"))
            {
                if(TutorialManager.Instance.GetCurrentPopUpIndex() == 13)
                    TutorialManager.Instance.popUps[13].SetActive(true);
                else if(TutorialManager.Instance.GetCurrentPopUpIndex() == 14)
                    TutorialManager.Instance.jumpWall = true;
            }
        }
    }
}
