using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "JumpKingJumpSkillStatus", menuName = "Config/Enemy/JumpKing/JumpKingJumpSkillStatus")]
    public class JumpKingJumpSkillConfig : ScriptableObject
    {
        public float horizontalDistanceEarthBump = 1f;
        public float horizontalDistanceFallingRock = 1f;
        public float jumpForce = 1f;
    }
}



