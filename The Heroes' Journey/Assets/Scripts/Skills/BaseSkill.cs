using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class BaseSkill : MonoBehaviour
    {
        public SkillConfig config;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void DealDamage()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, config.attackRange, config.targetLayer);
            if (hit != null)
                hit.gameObject.GetComponent<Player>().TakeDamage(config.damage);
        }

        public virtual void Move() { }

        public virtual void DestroySkill() 
        {
            SkillManager.Instance.Return(this, config.type);
        }
    }
}
