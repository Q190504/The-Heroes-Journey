using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class PlayerLightTrigger : MonoBehaviour
    {
        public Vector2 turnOnDirection;
        public Vector2 turnOffDirection;
        private Collider2D _collider;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player"))
            {
                Vector2 exitDirection = (collision.transform.position - _collider.bounds.center).normalized;
                if((turnOnDirection.x == -1 && exitDirection.x < 0 ) || (turnOnDirection.x == 1 && exitDirection.x > 0) || (turnOnDirection.y == -1 && exitDirection.y < 0) || (turnOnDirection.y == 1 && exitDirection.y > 0))
                    LightManager.Instance.TurnOnPlayerLight();
                else
                    LightManager.Instance.TurnOffPlayerLight();
            }
        }
    }
}


