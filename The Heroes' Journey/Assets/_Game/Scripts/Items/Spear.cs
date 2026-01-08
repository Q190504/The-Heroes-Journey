using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Spear : MonoBehaviour
    {
        public GameObject aura;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isSpearFound)
            {
                AudioManager.Instance.Play("CollectItem");

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                aura.SetActive(false);
                InGameManager.Instance.isSpearFound = true;
                UIManager.Instance.foundSpearPanel.SetActive(true);
                UIManager.Instance.displayObjList[1].SetActive(true);

                FirebaseManager.LogEvent("Collect_Spear");

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

