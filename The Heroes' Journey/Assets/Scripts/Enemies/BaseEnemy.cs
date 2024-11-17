using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace TheHeroesJourney
{
    public class BaseEnemy : BaseCharacter
    {
        public EnemyConfig enemyConfig;
        public EnemyType type;
        protected float currentSpeed;
        protected float stuntTimer;
        protected float attackCooldownTimer;


        protected Rigidbody2D rb;
        protected Animator anim;

        public bool isTakingDamage = false;
        public bool isAttacking = false;
        public bool isDying = false;

        protected Vector3 initScale;
        protected bool isWaiting = false;
        protected Transform currentPoint;
        public GameObject leftPoint;
        public GameObject rightPoint;

        public Player player;
        protected Transform playerPos;

        [SerializeField] private BoxCollider2D attackRangeCollider;
        [SerializeField] private BoxCollider2D chaseRangeCollider;
        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private BoxCollider2D edgeCheckCollider;
        [SerializeField] private LayerMask terrainLayer;
        protected bool canChangeDirection = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentPoint = leftPoint.transform;
            initScale = transform.localScale;
        }

        public virtual void Patrol()
        {
            if (!isWaiting)
            {
                currentSpeed = enemyConfig.patrolSpeed;

                int direction;
                if (transform.position.x < currentPoint.position.x)
                    direction = 1;
                else
                    direction = -1;

                transform.position = new Vector3(transform.position.x + Time.deltaTime * direction * enemyConfig.patrolSpeed, transform.position.y, transform.position.z);
            }

            if (Mathf.Abs(transform.position.x - currentPoint.position.x) < 0.5f && !isWaiting)
            {
                isWaiting = true;
                StartCoroutine(Wait());
            }
        }

        IEnumerator Wait()
        {
            currentSpeed = 0;
            yield return new WaitForSeconds(enemyConfig.waitTimeAfterReachCurrentPosition);
            currentPoint = (currentPoint == leftPoint.transform) ? rightPoint.transform : leftPoint.transform;
            isWaiting = false;
        }

        public void FaceTarget(Transform target)
        {
            int direction;
            if (transform.position.x < target.position.x)
                direction = 1;
            else
                direction = -1;

            transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(leftPoint.transform.position, 0.5f);
            Gizmos.DrawSphere(rightPoint.transform.position, 0.5f);
            Gizmos.DrawLine(leftPoint.transform.position, rightPoint.transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackRangeCollider.bounds.center - transform.right * enemyConfig.attackRange * transform.localScale.x * enemyConfig.attackColliderDistance,
                new Vector3(attackRangeCollider.bounds.size.x * enemyConfig.attackRange, attackRangeCollider.bounds.size.y, attackRangeCollider.bounds.size.z));

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(chaseRangeCollider.bounds.center - transform.right * enemyConfig.chaseRange * transform.localScale.x * enemyConfig.chaseColliderDistance,
                new Vector3(chaseRangeCollider.bounds.size.x * enemyConfig.chaseRange, chaseRangeCollider.bounds.size.y, chaseRangeCollider.bounds.size.z));
        }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Trap"))
                Die();
        }

        public override void Die()
        {
            isDying = true;
            anim.SetBool("isDying", true);
            anim.SetTrigger("die");
            StartCoroutine(Delay());
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2f);
            EnemyManager.Instance.Return(this, type);
        }

        public override void TakeDamage(float damageAmount)
        {
            if (currentHealth > 0)
            {
                isTakingDamage = true;
                currentSpeed = 0;
                anim.SetTrigger("hurt");

                base.TakeDamage(damageAmount);
                StartCoroutine(Stunt());
            }
        }

        IEnumerator Stunt()
        {
            yield return new WaitForSeconds(enemyConfig.stuntTime);
            currentSpeed = enemyConfig.chaseSpeed;
            isTakingDamage = false;
        }

        public virtual void Attack() { }

        public bool PlayerInAttackRange()
        {
            RaycastHit2D hit = Physics2D.BoxCast(attackRangeCollider.bounds.center - transform.right * enemyConfig.attackRange * transform.localScale.x * enemyConfig.attackColliderDistance,
                new Vector3(attackRangeCollider.bounds.size.x * enemyConfig.attackRange, attackRangeCollider.bounds.size.y, attackRangeCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
            return hit.collider != null;
        }

        public bool PlayerInChaseRange()
        {
            RaycastHit2D hit = Physics2D.BoxCast(chaseRangeCollider.bounds.center - transform.right * enemyConfig.chaseRange * transform.localScale.x * enemyConfig.chaseColliderDistance,
                new Vector3(chaseRangeCollider.bounds.size.x * enemyConfig.chaseRange, chaseRangeCollider.bounds.size.y, chaseRangeCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
            return hit.collider != null;
        }
    }
}