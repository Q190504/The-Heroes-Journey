using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = " NPCStats", menuName = "Config/NPC")]

    public class NPCConfig : ScriptableObject
    {
        public float moveSpeed = 0;
        public float waitTimeAfterReachCurrentPosition = 1;
    }
}
