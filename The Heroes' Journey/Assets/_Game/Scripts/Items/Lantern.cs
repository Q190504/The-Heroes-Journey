using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Lantern : MonoBehaviour
    {
        public GameObject aura;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isLanternFound)
            {
                AudioManager.Instance.Play("CollectItem");

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                aura.SetActive(false);
                InGameManager.Instance.isLanternFound = true;
                UIManager.Instance.foundLanternPanel.SetActive(true);
                UIManager.Instance.displayObjList[3].SetActive(true);

                FirebaseManager.LogEvent("Collect_Lantern");

                StartCoroutine(SaveGame());
            }
        }

        IEnumerator SaveGame()
        {
            InGameManager.Instance.respawnPos = InGameManager.Instance.player.transform.position;
            DataPresistenceManager.Instance.SaveGame();
            yield return new WaitForSeconds(2);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            aura.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

