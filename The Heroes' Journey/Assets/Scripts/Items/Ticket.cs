using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Ticket : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null && collision.CompareTag("Player") && !InGameManager.Instance.isTicketFound)
            {
                AudioManager.Instance.Play("CollectItem");
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                InGameManager.Instance.isTicketFound = true;
                UIManager.Instance.foundJejuTicketPanel.SetActive(true);


                FirebaseManager.LogEvent("Collect_Ticket");

                StartCoroutine(DisableObject());
            }
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.SetActive(false);
        }
    }
}
