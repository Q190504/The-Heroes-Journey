using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Thunder : MonoBehaviour
    {
        public GameObject aura;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isThunderFound)
            {
                AudioManager.Instance.Play("CollectItem");
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                aura.SetActive(false);
                InGameManager.Instance.isThunderFound = true;
                UIManager.Instance.foundThunderPanel.SetActive(true);
                UIManager.Instance.displayObjList[0].SetActive(true);
                UIManager.Instance.SetActiveCultistPriestlEntranceWall(false);

                FirebaseManager.LogEvent("Collect_Thunder");

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

