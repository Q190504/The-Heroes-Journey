using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class EarthBump : BaseSkill
    {
        public void PlayDisappearSound()
        {
            AudioManager.Instance.Play("EarthBumpDissappear");
        }

        public override void DestroySkill()
        {
            SkillManager.Instance.Return(this, SkillType.EarthBump);
        }
    }
}

