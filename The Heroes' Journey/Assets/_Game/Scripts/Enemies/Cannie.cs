using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Cannnie : BaseEnemy
    {
        // Start is called before the first frame update
        void Start()
        {
            currentHealth = enemyConfig.maxHealth;
            currentSpeed = enemyConfig.patrolSpeed;

            attackCooldownTimer = 0;
            stuntTimer = 0;
            player = InGameManager.Instance.player;
            playerPos = player.GetComponent<Transform>();
        }


        // Update is called once per frame
        void Update()
        {
            anim.SetFloat("currentSpeed", currentSpeed);

            if (isDying)
                rb.linearVelocity = Vector2.zero;
            else if (!isTakingDamage)
            {
                if (PlayerInAttackRange())
                {
                    if (!isAttacking && attackCooldownTimer <= 0 && currentHealth > 0)
                    {
                        FaceTarget(playerPos);
                        isAttacking = true;
                        rb.linearVelocity = new Vector2(0, 0);
                        currentSpeed = 0;

                        anim.SetTrigger("attack");
                    }
                    else
                    {
                        attackCooldownTimer -= Time.deltaTime;
                    }
                }
                else if (!isAttacking && PlayerInChaseRange() && Vector2.Distance(playerPos.position, rb.position) > enemyConfig.attackRange)
                {
                    FaceTarget(playerPos);
                    currentSpeed = enemyConfig.chaseSpeed;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.position.x, transform.position.y), enemyConfig.chaseSpeed * Time.deltaTime);

                    if (attackCooldownTimer > 0)
                        attackCooldownTimer -= Time.deltaTime;
                }
                else if(!isAttacking)
                {
                    FaceTarget(currentPoint);
                    Patrol();
                }
            }
        }
        public override void Attack()
        {
            AudioManager.Instance.Play("CannieAttack");

            if (PlayerInAttackRange())
                InGameManager.Instance.player.TakeDamage(enemyConfig.damage);

            currentSpeed = 0;
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(enemyConfig.attackCooldown);
            attackCooldownTimer = enemyConfig.attackCooldown;
            isAttacking = false;
        }

        public override void TakeDamage(float damageAmount)
        {
            if (InGameManager.Instance.isGameFinished || damageAmount <= 0 || currentHealth <= 0)
                return;
            AudioManager.Instance.Play("CannieHurt");
            base.TakeDamage(damageAmount);
        }

        public override void Die()
        {
            AudioManager.Instance.Play("CannieDie");

            base.Die();
        }
    }
}
