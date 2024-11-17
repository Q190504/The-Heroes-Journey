using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace TheHeroesJourney
{
    public class BaseCharacter : MonoBehaviour, IDamageable
    {
        protected float currentHealth;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void TakeDamage(float damageAmount)
        {
            if (damageAmount <= 0 || currentHealth <= 0) return;

            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }
        public virtual void Die() { }
    }
}

