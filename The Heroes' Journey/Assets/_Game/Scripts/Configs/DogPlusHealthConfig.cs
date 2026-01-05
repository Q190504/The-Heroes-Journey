using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "DogPlusHealthStats", menuName = "Config/Character/DogPlusHealthStats")]

    public class DogPlusHealthConfig : ScriptableObject
    {
        public float healthPlusAmount = 1;
    }
}
