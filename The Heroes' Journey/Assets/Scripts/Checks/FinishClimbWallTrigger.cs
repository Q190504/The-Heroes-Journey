using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class FinishClimbWallTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player") && TutorialManager.Instance != null)
                TutorialManager.Instance.climbWall = true;
        }
    }
}
