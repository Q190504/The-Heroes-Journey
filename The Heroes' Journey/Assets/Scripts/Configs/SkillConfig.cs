using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "SkillStatus", menuName = "Config/Skills/SkillStatus")]

    public class SkillConfig : ScriptableObject
    {
        public SkillType type;
        public int damage = 7;
        public float attackRange = 3;
        public LayerMask targetLayer;
    }
}


