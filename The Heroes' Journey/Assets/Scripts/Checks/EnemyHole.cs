using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

namespace TheHeroesJourney
{
    public class EnemyHole : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < -1)
                collision.GetComponent<BaseEnemy>().TakeDamage(Mathf.Infinity);
        }
    }
}