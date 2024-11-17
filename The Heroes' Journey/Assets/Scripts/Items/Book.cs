using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Book : MonoBehaviour
    {
        public GameObject aura;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isBookFound)
            {
                AudioManager.Instance.Play("CollectItem");

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                aura.SetActive(false);
                InGameManager.Instance.isBookFound = true;
                InGameManager.Instance.isFourObjectsFound = true;
                UIManager.Instance.foundBookPanel.SetActive(true);
                foreach(GameObject displayObj in UIManager.Instance.displayObjList)
                    displayObj.SetActive(true);

                FirebaseManager.LogEvent("Collect_Book");
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

