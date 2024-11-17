using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using UnityEngine.Playables;

namespace TheHeroesJourney
{
    public class Fairy : NPC
    {
        public PlayableDirector attackAtMarketCutscene;

        private void Start()
        {
            initScale = transform.localScale;
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            FaceTarget(InGameManager.Instance.player.transform);
        }

        public void FairyDissapear()
        {
            anim.SetBool("isFlying", false);
            this.gameObject.SetActive(false);
            UIManager.Instance.brigde.SetActive(true);
        }
    }
}

