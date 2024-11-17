using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class NPC : MonoBehaviour
    {
        protected bool isTalking = false;
        protected bool isFlipped;
        protected Transform currentPoint;
        public GameObject leftPoint;
        public GameObject rightPoint;

        protected Animator anim;
        protected Rigidbody2D rb;

        public NPCConfig config;

        public GameObject talkPopupEmote;

        protected Vector3 initScale;

        private bool isWaiting = false;

        public void Patrol()
        {
            if (!isWaiting)
            {
                int direction;
                if (transform.position.x < currentPoint.position.x)
                    direction = 1;
                else
                    direction = -1;

                if(talkPopupEmote != null)
                    talkPopupEmote.transform.position = new Vector3(talkPopupEmote.transform.position.x + Time.deltaTime * direction * config.moveSpeed, talkPopupEmote.transform.position.y, talkPopupEmote.transform.position.z);
                transform.position = new Vector3(transform.position.x + Time.deltaTime * direction * config.moveSpeed, transform.position.y, transform.position.z);
            }

            if (Mathf.Abs(transform.position.x - currentPoint.position.x) < 0.5f && !isWaiting)
            {
                isWaiting = true;
                StartCoroutine(Wait());
            }
        }

        IEnumerator Wait()
        {
            anim.SetBool("isMoving", false);
            yield return new WaitForSeconds(config.waitTimeAfterReachCurrentPosition);
            currentPoint = (currentPoint == leftPoint.transform) ? rightPoint.transform : leftPoint.transform;
            isWaiting = false;
            anim.SetBool("isMoving", true);
        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player"))
            {
                if(talkPopupEmote != null)
                    talkPopupEmote.SetActive(true);

                if (this.gameObject.CompareTag("Kitsune"))
                {
                    anim = GetComponent<Animator>();
                    anim.SetBool("isBlocking", true);
                }
                else if (this.gameObject.CompareTag("Fairy"))
                {
                    if (TutorialManager.Instance != null && TutorialManager.Instance.GetCurrentPopUpIndex() == 5)
                    {
                        TutorialManager.Instance.switchCharacterPanel.SetActive(true);
                        TutorialManager.Instance.skillButton.SetActive(true);
                        TutorialManager.Instance.popUps[5].SetActive(true);
                    }
                }

                if (InGameManager.Instance.player._currentCharacterIndex == 0 && UIManager.Instance.skillButton != null)
                    UIManager.Instance.TurnOnSkillButton();
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player"))
            {
                if (talkPopupEmote != null)
                    talkPopupEmote.SetActive(true);

                if (this.gameObject.CompareTag("Kitsune"))
                {
                    anim = GetComponent<Animator>();
                    anim.SetBool("isBlocking", false);
                }
                if (InGameManager.Instance.player._currentCharacterIndex == 0 && UIManager.Instance.skillButton != null)
                    UIManager.Instance.TurnOffSkillButton();
            }
        }

        public void FaceTarget(Transform target)
        {
            int direction;
            if (transform.position.x < target.position.x)
                direction = 1;
            else
                direction = -1;

            transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        }

        public void Talk()
        {
            isTalking = true;
        }

        public void NPCDisappear()
        {
            StartCoroutine(DieCoroutine());
        }

        IEnumerator DieCoroutine()
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("die");
            yield return new WaitForSeconds(1);
            this.gameObject.SetActive(false);
            talkPopupEmote.SetActive(false);
        }
    }
}

