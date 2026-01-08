using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = " MeleeAttackStats", menuName = "Config/MeleeAttackStats")]

    public class MeleeAttackConfig : ScriptableObject
    {
        public float timeBtwAttack = 0;
        public float attackRadius = 0;
        public float damage = 0;
    }
}
