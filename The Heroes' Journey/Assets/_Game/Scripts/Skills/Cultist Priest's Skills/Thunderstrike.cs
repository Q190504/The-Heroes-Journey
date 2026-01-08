using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Thunderstrike : BaseSkill
    {
        public override void DealDamage()
        {
            AudioManager.Instance.Play("ThunderstrikeImpact");

            Vector3 hitPos = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
            Collider2D hit = Physics2D.OverlapCircle(hitPos, config.attackRange, config.targetLayer);
            if (hit != null)
                hit.gameObject.GetComponent<Player>().TakeDamage(config.damage);
        }
    }
}
