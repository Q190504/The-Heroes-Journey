using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "ThunderBirdBulletStatus", menuName = "Config/Bullet/ThunderBirdBulletStatus")]

    public class ThunderBirdBulletConfig : ScriptableObject
    {
        public float createTime = 0.5f;
        public float bindingTime = 1f;
    }
}



