using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "CatShootStats", menuName = "Config/Character/CatShootStats")]
    public class CatShootConfig : ScriptableObject
    {
        public float bulletInterval = 0;
    }
}
