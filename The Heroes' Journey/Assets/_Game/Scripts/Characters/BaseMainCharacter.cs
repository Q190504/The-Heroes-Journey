using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace TheHeroesJourney
{
    public class BaseMainCharacter : MonoBehaviour
    {
        protected Player _host;
        public SkillCooldownConfig skillCooldownConfig;

        public Player player;

        public virtual void SetPlayerHost(Player p)
        {
            this._host = p;
        }

        public virtual void ActiveSkill() { }
    }
}

