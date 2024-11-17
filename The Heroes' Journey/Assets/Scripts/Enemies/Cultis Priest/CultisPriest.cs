using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


namespace TheHeroesJourney
{
    enum CultisPriestSkills
    {
        Thunderbird,
        Thunderstrike
    }

    public class CultisPriest : BaseEnemy
    {
        public SkillCooldownConfig skillCooldownConfig;
        private ShootThunderBirdBullet shootThunderBirdBullet;
        private CreateThunderstrike createThunderstrike;

        private CultisPriestSkills currentSkill;
        private Random random = new Random();


        // Start is called before the first frame update
        void Start()
        {
            shootThunderBirdBullet = GetComponent<ShootThunderBirdBullet>();
            createThunderstrike = GetComponent<CreateThunderstrike>();

            player = InGameManager.Instance.player;
            playerPos = player.GetComponent<Transform>();
            currentHealth = enemyConfig.maxHealth;
            currentSpeed = enemyConfig.patrolSpeed;
            attackCooldownTimer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            anim.SetFloat("currentSpeed", currentSpeed);

            if (isDying)
            {
                currentSpeed = 0;
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                if (!isTakingDamage)
                {
                    if (Vector2.Distance(playerPos.position, rb.position) <= enemyConfig.attackRange)
                    {
                        if (!isAttacking && attackCooldownTimer <= 0)
                        {
                            FaceTarget(playerPos);

                            isAttacking = true;

                            currentSpeed = 0;
                            rb.velocity = new Vector2(0, 0);

                            Attack();
                        }
                        else
                            attackCooldownTimer -= Time.deltaTime;
                    }
                    else if (!isAttacking && InGameManager.Instance.isBattleCultisPriest && PlayerInChaseRange() && Vector2.Distance(playerPos.position, rb.position) > enemyConfig.attackRange && !isTakingDamage)
                    {
                        FaceTarget(playerPos);

                        currentSpeed = enemyConfig.chaseSpeed;
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.position.x, transform.position.y), enemyConfig.chaseSpeed * Time.deltaTime);

                        if (attackCooldownTimer > 0)
                            attackCooldownTimer -= Time.deltaTime;
                    }
                    else if (!isAttacking)
                    {
                        FaceTarget(currentPoint);
                        Patrol();
                        currentSpeed = enemyConfig.patrolSpeed;
                    }
                }
            }
        }

        public override void Attack()
        {
            anim.SetTrigger("attack");
            rb.velocity = new Vector2(0, 0);
            AudioManager.Instance.Play("CultistPriestAttack");
            currentSpeed = 0;


            Array enumValues = Enum.GetValues(typeof(CultisPriestSkills));

            int minIndex = (int)CultisPriestSkills.Thunderbird;
            int maxIndex = (int)CultisPriestSkills.Thunderstrike;
            currentSkill = (CultisPriestSkills)enumValues.GetValue(random.Next(minIndex, maxIndex + 1));

            switch (currentSkill)
            {
                case CultisPriestSkills.Thunderbird:
                    shootThunderBirdBullet.ThunderBirdBullet();
                    break;
                case CultisPriestSkills.Thunderstrike:
                    createThunderstrike.ThunderstrikeAbility();
                    break;
                default:
                    break;
            }
            attackCooldownTimer = skillCooldownConfig.skillCooldown;
        }

        public override void TakeDamage(float damageAmount)
        {
            if (InGameManager.Instance.isGameFinished || damageAmount <= 0 || currentHealth <= 0)
                return;
            AudioManager.Instance.Play("CultistPriestHurt");

            anim.SetTrigger("hurt");
            stuntTimer = enemyConfig.stuntTime;
            base.TakeDamage(damageAmount);
        }

        public override void Die()
        {
            isDying = true;
            AudioManager.Instance.Play("CultistPriestDie");
            anim.SetTrigger("die");
            StartCoroutine(Delay());
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2f);
            EnemyManager.Instance.Return(this, type);

            UIManager.Instance.thunder.SetActive(true);
            UIManager.Instance.thunder.transform.position = transform.position;
            InGameManager.Instance.isBattleCultisPriest = false;

            AudioManager.Instance.Stop("BattleCultisPriestTheme");
            AudioManager.Instance.Play("MainGameTheme");

            FirebaseManager.LogEvent("CultisPriest_Deafeat");
        }

        public bool IsBelowHalfHealth()
        {
            return currentHealth <= enemyConfig.maxHealth * 0.5;
        }
    }
}
