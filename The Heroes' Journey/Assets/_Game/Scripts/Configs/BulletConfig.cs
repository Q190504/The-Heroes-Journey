using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "BulletStatus", menuName = "Config/Bullet/NormBulletStatus")]

    public class BulletConfig : ScriptableObject
    {
        public BulletType type = 0;
        public float normalSpeed = 1;
        public float speedAfterThreshold = 1;
        public float angle = 0;
        public float maxRange = 5f;
        public float thresholdDistance = 1.5f;
        public int damage = 1;
        public string targetTag;
    }
}


