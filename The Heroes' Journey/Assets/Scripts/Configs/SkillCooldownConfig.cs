using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "SkillCooldownStats", menuName = "Config/Character/SkillCooldownStats")]
    public class SkillCooldownConfig : ScriptableObject
    {
        public float cooldownTimer = 0;
        public float skillCooldown = 0;
    }
}