using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class TreeRootBidingEffect : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void DissapearAnimation()
        {
            AudioManager.Instance.Play("TreeRootDisappear");

            _animator.SetTrigger("dissapear");
        }

        public void Return()
        {
            EffectManager.Instance.Return(gameObject);
        }
    }
}

