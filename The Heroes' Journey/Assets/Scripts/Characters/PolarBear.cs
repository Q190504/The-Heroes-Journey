using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class PolarBear : BaseMainCharacter
    {
        public PolarBearPlusHealthConfig polarBearPlusHealthConfig;

        // Start is called before the first frame update
        void Start()
        {
            player = InGameManager.Instance.player;
            skillCooldownConfig.cooldownTimer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(skillCooldownConfig.cooldownTimer <= 0f && !InGameManager.Instance.isGameFinished)
            {
                skillCooldownConfig.cooldownTimer = skillCooldownConfig.skillCooldown;
                Skill();
            }
            else
                skillCooldownConfig.cooldownTimer -= Time.deltaTime;
        }

        private void Skill()
        {
            player.HealByPolarBear(polarBearPlusHealthConfig.additionalHealthAmount);
        }
    }
}

