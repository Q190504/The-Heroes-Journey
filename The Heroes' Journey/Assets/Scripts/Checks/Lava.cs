using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

namespace TheHeroesJourney
{
    public class LavaCheck : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    FirebaseManager.LogEvent("Player_DieLava");
                    InGameManager.Instance.player.TakeDamage(Mathf.Infinity);
                }
                else if (collision.gameObject.CompareTag("Enemy"))
                    collision.GetComponent<BaseEnemy>().TakeDamage(Mathf.Infinity);
            }
        }
    }

}
