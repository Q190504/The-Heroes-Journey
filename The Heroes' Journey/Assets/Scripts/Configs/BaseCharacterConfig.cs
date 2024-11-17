using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "BaseCharacterStats", menuName = "Config/BaseCharacter")]
    public class BaseCharacterConfig : ScriptableObject
    {
        public float maxHealth = 1;
    }
}

