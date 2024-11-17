using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Config/Enemy/EnemyStatus")]
    public class EnemyConfig : BaseCharacterConfig
    {
        public float damage = 1;
        public float attackCooldown = 1;
        public float attackRange = 1;
        public float attackColliderDistance = 1;

        public float chaseRange = 1;
        public float chaseColliderDistance = 0;
        public float chaseSpeed = 1;

        public float patrolSpeed = 1;
        public float waitTimeAfterReachCurrentPosition = 1;
        public float stuntTime = 1;

    }
}


