using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "PolarBearPlusHealthStats", menuName = "Config/Character/PolarBearPlusHealthStats")]
    public class PolarBearPlusHealthConfig : ScriptableObject
    {
        public float additionalHealthAmount = 0;
    }
}
