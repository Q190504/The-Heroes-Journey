using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Penguin : BaseMainCharacter
    {
        bool isPlayerFacingRight;

        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            skillCooldownConfig.cooldownTimer = 0;
        }

        public override void ActiveSkill()
        {
            //if (skillCooldownConfig.cooldownTimer <= 0)
            //{
            //    if (TutorialManager.Instance.GetCurrentPopUpIndex() == 9)
            //        TutorialManager.Instance.penguinSkillPressed = true;

            //    AudioManager.Instance.Play("PenguinThrow");
            //    anim.SetTrigger("shoot");
            //    isPlayerFacingRight = InGameManager.Instance.player.move.isFacingRight;
            //    BaseBullet trophy = BulletManager.Instance.Take(BulletType.Trophy);
            //    trophy.transform.SetLocalPositionAndRotation(transform.position, Quaternion.Euler(0, 0, trophy.config.angle));
            //    trophy.bulletMoveDirection = isPlayerFacingRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
            //    skillCooldownConfig.cooldownTimer = skillCooldownConfig.skillCooldown;
            //    base.ActiveSkill();
            //}
        }
    }
}

