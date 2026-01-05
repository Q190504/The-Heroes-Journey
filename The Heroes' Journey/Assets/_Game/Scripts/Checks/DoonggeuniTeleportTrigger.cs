using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class DoonggeuniTeleportTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null && collision.CompareTag("Player"))
                InGameManager.Instance.tutorialDoonggeuni.transform.position = InGameManager.Instance.tutorialDoonggeuniTeleportPosition.transform.position;
        }
    }
}

