using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class WindBullet : BaseBullet
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
            if (distanceTraveled >= config.maxRange)
            {
                AudioManager.Instance.Play("WindBulletHit");

                DestroyBullet();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(config.targetTag))
            {
                AudioManager.Instance.Play("WindBulletHit");

                if (animator != null)
                    animator.SetTrigger("hit");

                if (collision.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(config.damage);
                    DestroyBullet();
                }
                else if (collision.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Player>().TakeDamage(config.damage);
                    DestroyBullet();
                }
            }
            else if (collision.CompareTag("Terrain"))
            {
                DestroyBullet();
            }
        }

        public override void Move()
        {
            if (distanceTraveled >= config.thresholdDistance)
            {
                transform.position += config.speedAfterThreshold * Time.deltaTime * bulletMoveDirection;
                distanceTraveled += config.speedAfterThreshold * Time.deltaTime;
            }
            else
            {
                transform.position += config.normalSpeed * Time.deltaTime * bulletMoveDirection;
                distanceTraveled += config.normalSpeed * Time.deltaTime;
            }
        }
    }
}

