using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class CultistPriestFightTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isBattleCultisPriest && UIManager.Instance.cultisPriest.activeSelf)
            {
                UIManager.Instance.cultistPriestlEntranceWall.SetActive(true);
                AudioManager.Instance.Stop("MainGameTheme");

                AudioManager.Instance.Play("BattleCultisPriestTheme");
                InGameManager.Instance.isBattleCultisPriest = true;
            }
        }
    }
}

