using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "TigerDashStats", menuName = "Config/Character/TigerDashStats")]

    public class TigerDashConfig : ScriptableObject
    {
        public float dashingPower = 15f;
        public float dashingTime = 0.2f;
    }
}
