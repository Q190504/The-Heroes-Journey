using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class WindExplotion : BaseSkill
    {
        private void Update()
        {
            if (InGameManager.Instance.isGameFinished)
                DestroySkill();
        }

        public override void DealDamage()
        {
            AudioManager.Instance.Play("WindExplotion");
            base.DealDamage();
        }
    }
}

