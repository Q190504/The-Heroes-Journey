using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Hotpot : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && TutorialManager.Instance != null && !TutorialManager.Instance.hotPotFound)
            {
                AudioManager.Instance.Play("CollectItem");
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                UIManager.Instance.foundHotpotPanel.SetActive(true);
                TutorialManager.Instance.hotPotFound = true;

                FirebaseManager.LogEvent("Collect_Hotpot");
                StartCoroutine(SaveGame());
            }
        }

        IEnumerator SaveGame()
        {
            yield return new WaitForSeconds(2);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.SetActive(false);
        }
    }
}

