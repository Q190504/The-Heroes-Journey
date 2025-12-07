using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheHeroesJourney
{
    public class ThunderBirdBullet : BaseBullet
    {
        public ThunderBirdBulletConfig thunderBirdBulletConfig;

        private bool isHit;
        private Collider2D target;
        private float timer;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            timer = 0f;
            isHit = false;
            target = null;

            StartCoroutine(Create());
        }

        // Update is called once per frame
        void Update()
        {
            if (distanceTraveled >= config.maxRange)
            {
                isHit = false;
                distanceTraveled = 0f;
                timer = 0;
                DestroyBullet();
            }
            if(!isHit)
                Move();

            if (isHit)
            {
                AudioManager.Instance.Play("TreeRootAppear");

                GameObject bidingEffect = EffectManager.Instance.Take(EffectType.BidingEffect);
                if (timer <= thunderBirdBulletConfig.bindingTime)
                {
                    bidingEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + 0.61f, target.transform.position.z);

                    SlowTarget();
                    timer += Time.deltaTime;
                }
                else
                {
                    bidingEffect.GetComponent<TreeRootBidingEffect>().DissapearAnimation();
                    isHit = false;
                    distanceTraveled = 0f;
                    timer = 0;
                    DestroyBullet();
                }
            }
        }
        public override void Move()
        {
            transform.position += config.normalSpeed * Time.deltaTime * bulletMoveDirection;
            distanceTraveled += config.normalSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag(config.targetTag))
            {
                animator.SetTrigger("hit");

                collision.GetComponent<Player>().TakeDamage(config.damage);

                isHit = true;
                target = collision;
            }
            else if (collision.CompareTag("Terrain"))
            {

                animator.SetTrigger("hit");
                DestroyBullet();
            }
        }

        private void SlowTarget()
        {
            target.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }

        IEnumerator Create()
        {
            yield return new WaitForSeconds(thunderBirdBulletConfig.createTime);
            AudioManager.Instance.Play("ThunderBirdBullet");
            animator.SetBool("isFlying", true);
        }

        public override void DestroyBullet()
        {
            AudioManager.Instance.Stop("ThunderBirdBullet");

            base.DestroyBullet();
        }
    }
}
