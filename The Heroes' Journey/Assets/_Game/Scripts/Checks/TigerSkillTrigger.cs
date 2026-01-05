using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class TigerSkillTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision!= null && collision.CompareTag("Player") && TutorialManager.Instance != null && TutorialManager.Instance.GetCurrentPopUpIndex() == 11)
                TutorialManager.Instance.popUps[12].SetActive(true);
        }
    }
}
