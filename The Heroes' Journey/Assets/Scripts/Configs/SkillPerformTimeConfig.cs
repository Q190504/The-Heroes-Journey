using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "DelayAfterFinishTimeStatus", menuName = "Config/Skills/DelayAfterFinishTimeStatus")]

    public class DelayAfterFinishTimeConfig : ScriptableObject
    {
        public float delayAfterFinishTime = 2;
    }
}


