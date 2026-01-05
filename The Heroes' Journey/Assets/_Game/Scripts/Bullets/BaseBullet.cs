using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public abstract class BaseBullet : MonoBehaviour
    {
        public BulletConfig config;
        public Vector3 bulletMoveDirection;

        protected Animator animator;

        protected float distanceTraveled;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            this.transform.rotation = Quaternion.Euler(0, 0, config.angle);
            distanceTraveled = 0;
        }

        // Update is called once per frame
        void Update()
        {
            DestroyIfOutOfCameraView();
        }

        public virtual void Move() { }

        public void DestroyIfOutOfCameraView()
        {
            Vector3 objectPosition = transform.position;

            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(objectPosition);

            if (viewportPosition.x < 0 || viewportPosition.x > 1 ||
                viewportPosition.y < 0 || viewportPosition.y > 1 ||
                viewportPosition.z < 0)
                DestroyBullet();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(config.targetTag))
            {
                if(animator != null)
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

        public virtual void DestroyBullet()
        {
            distanceTraveled = 0;
            BulletManager.Instance.Return(this, config.type);
        }
    }
}
