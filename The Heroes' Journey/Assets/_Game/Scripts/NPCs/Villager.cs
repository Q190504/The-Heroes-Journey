using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Villager : NPC
    {
        public GameObject target;

        private bool seePlayer = false;

        private void Awake()
        {
            initScale = transform.localScale;
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            FaceTarget(target.transform);
            if (seePlayer)
                RunAway();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (InGameManager.Instance.player._currentCharacterIndex == 0 && UIManager.Instance.skillButton != null)
                    UIManager.Instance.TurnOffSkillButton();
                seePlayer = true;
            }
        }

        void RunAway()
        {
            int direction;
            if (transform.position.x < target.transform.position.x)
                direction = 1;
            else
                direction = -1;

            transform.position = new Vector3(transform.position.x + Time.deltaTime * direction * config.moveSpeed, transform.position.y, transform.position.z);

            if (Mathf.Abs(transform.position.x - target.transform.position.x) <= 0.1f)
                gameObject.SetActive(false);
        }
    }
}

