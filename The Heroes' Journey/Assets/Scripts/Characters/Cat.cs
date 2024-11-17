using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Cat : BaseMainCharacter
    {
        public CatShootConfig catShootStatsConfig;
        bool isPlayerFacingRight;

        private Animator anim;

        private void Awake()
        {
            skillCooldownConfig.cooldownTimer = 0;
            anim = GetComponent<Animator>();
        }


        public override void ActiveSkill()
        {
            MultiShot_With_Coroutine_Logic();
            base.ActiveSkill();
        }

        private void MultiShot_With_Coroutine_Logic()
        {
            if (skillCooldownConfig.cooldownTimer <= 0f)
            {
                if(TutorialManager.Instance.GetCurrentPopUpIndex() == 11)
                    TutorialManager.Instance.catSkillPressed = true;
                anim.SetTrigger("shoot");
                AudioManager.Instance.Play("CatShoot");
                StartCoroutine(Shoot());
                isPlayerFacingRight = InGameManager.Instance.player.move.isFacingRight;
                skillCooldownConfig.cooldownTimer = skillCooldownConfig.skillCooldown;
            }
        }
        IEnumerator Shoot()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(catShootStatsConfig.bulletInterval);

                BaseBullet bullet = BulletManager.Instance.Take(0);
                if (isPlayerFacingRight)
                {
                    bullet.bulletMoveDirection = new Vector3(1, 0, 0);
                    bullet.transform.SetLocalPositionAndRotation(transform.position - new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, bullet.config.angle));
                }
                else
                {
                    bullet.bulletMoveDirection = new Vector3(-1, 0, 0);
                    bullet.transform.SetLocalPositionAndRotation(transform.position - new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, bullet.config.angle - 180));
                }
            }
        }
    }
}

