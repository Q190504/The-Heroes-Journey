using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using Random = System.Random;

namespace TheHeroesJourney
{
    enum JumpKingSkills
    {
        WindBullet,
        WindExplotion,
        Jump
    }

    public class JumpKing : BaseEnemy
    {
        public SkillCooldownConfig skillCooldownConfig;

        public LayerMask groundLayer;
        public float groundedRayLength = 5f; // How far the ray extends below the object
        public bool onGround;
        public bool isJumping;

        public PlayableDirector victoryCutscene;
        private bool isInCutscene;

        private JumpKingSkills currentSkill;
        private ShootWindBullet shootWindBulletSkill;
        private CreateWindExplotion createWindExplotion;
        private JumpKingJumpSkill jumpSkill;

        private Random random = new Random();
        public bool canJumpLeft;
        public bool canJumpRight;

        // Start is called before the first frame update
        void Start()
        {
            shootWindBulletSkill = GetComponent<ShootWindBullet>();
            createWindExplotion = GetComponent<CreateWindExplotion>();
            jumpSkill = GetComponent<JumpKingJumpSkill>();

            SetDefaultStats();

            playerPos = player.transform;

            attackCooldownTimer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDying)
                return;
            onGround = IsGrounded();

            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            anim.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));

            attackCooldownTimer -= Time.deltaTime;

            if (!isInCutscene && !isJumping && !isAttacking && !isTakingDamage)
            {
                FaceTarget(playerPos);

                Vector2 directionToPlayer = ((Vector2)playerPos.position - (Vector2)transform.position).normalized;
                rb.linearVelocity = new Vector2(directionToPlayer.x * enemyConfig.chaseSpeed, 0);

                if (Vector2.Distance(playerPos.position, rb.position) <= enemyConfig.attackRange)
                {
                    if (attackCooldownTimer <= 0)
                    {
                        Attack();
                        attackCooldownTimer = skillCooldownConfig.skillCooldown;
                    }
                }
            }
        }

        public override void Attack()
        {
            isAttacking = true;

            Array enumValues = Enum.GetValues(typeof(JumpKingSkills));

            int minIndex = (int)JumpKingSkills.WindBullet;
            int maxIndex = (int)JumpKingSkills.Jump;
            currentSkill = (JumpKingSkills)enumValues.GetValue(random.Next(minIndex, maxIndex + 1));

            switch (currentSkill)
            {
                case JumpKingSkills.WindBullet:
                    anim.SetTrigger("attack");
                    rb.linearVelocity = new Vector2(0, 0);

                    shootWindBulletSkill.WindBullet();
                    break;
                case JumpKingSkills.WindExplotion:
                    anim.SetTrigger("attack");
                    rb.linearVelocity = new Vector2(0, 0);

                    createWindExplotion.WindExplotion();
                    break;
                case JumpKingSkills.Jump:
                    bool earthBump;
                    bool fallingRock;
                    if (currentHealth <= enemyConfig.maxHealth / 2)
                    {
                        earthBump = true;
                        fallingRock = true;
                    }
                    else
                    {
                        if (random.Next(0, 2) == 0)
                        {
                            earthBump = true;
                            fallingRock = false;
                        }
                        else
                        {
                            earthBump = false;
                            fallingRock = true;
                        }
                    }

                    jumpSkill.JumpSkill(earthBump, fallingRock);
                    break;
                default:
                    break;
            }
        }

        public bool IsGrounded()
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + groundedRayLength, groundLayer);

            return hit.collider != null;
        }

        public override void TakeDamage(float damageAmount)
        {
            if (InGameManager.Instance.isGameFinished || damageAmount <= 0 || currentHealth <= 0)
                return;
            AudioManager.Instance.Play("JumpKingHurt");

            anim.SetTrigger("hurt");
            stuntTimer = enemyConfig.stuntTime;
            base.TakeDamage(damageAmount);
        }

        public override void Die()
        {
            currentSpeed = 0;
            isDying = true;
            rb.linearVelocity = Vector3.zero;
            AudioManager.Instance.Play("JumpKingSmack");
            AudioManager.Instance.Play("JumpKingDie");
            anim.SetTrigger("die");
            base.Die();

            InGameManager.Instance.isGameFinished = true;

            DataPresistenceManager.Instance.SaveGame();

            FirebaseManager.LogEvent("JumpKing_Defeat");

            UIManager.Instance.EndingGameDisplay();

            victoryCutscene.Play();
        }

        public void StartEncounterCutscene()
        {
            isInCutscene = true;
        }
        public void EndEncounterCutscene()
        {
            isInCutscene = false;
        }

        public void SetDefaultStats()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;
            currentHealth = enemyConfig.maxHealth;
            currentSpeed = enemyConfig.patrolSpeed;
            canJumpLeft = true;
            canJumpRight = true;
            onGround = false;
            isJumping = true;
            attackCooldownTimer = 0;
        }
    }
}


