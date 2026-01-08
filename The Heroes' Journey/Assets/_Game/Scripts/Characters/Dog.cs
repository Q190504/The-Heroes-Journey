using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Dog : BaseMainCharacter
    {
        public DogPlusHealthConfig plusHealthConfig;

        private Animator anim;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            skillCooldownConfig.cooldownTimer = 0;

        }

        public override void ActiveSkill()
        {
            if(skillCooldownConfig.cooldownTimer <= 0)
            {
                if (TutorialManager.Instance.GetCurrentPopUpIndex() == 9)
                    TutorialManager.Instance.dogSkillPressed = true;

                AudioManager.Instance.Play("DogHeal");
                anim.SetTrigger("heal");
                InGameManager.Instance.player.isDogHealing = true;
                _host.HealByDog(plusHealthConfig.healthPlusAmount);
                skillCooldownConfig.cooldownTimer = skillCooldownConfig.skillCooldown;
                base.ActiveSkill();
            }
        }
    }
}
