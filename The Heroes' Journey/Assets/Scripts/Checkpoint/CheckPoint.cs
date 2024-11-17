using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

namespace TheHeroesJourney
{
    public class CheckPoint : MonoBehaviour
    {
        public GameObject glow_sprite;
        public int checkPointID;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InGameManager.Instance.currentCheckPointID = checkPointID;
                InGameManager.Instance.currentCheckPointPos = this.transform.position;
                UIManager.Instance.saveGameButton.gameObject.SetActive(true);

                if (checkPointID == 0 && TutorialManager.Instance != null && TutorialManager.Instance.GetCurrentPopUpIndex() == 3)
                {
                    TutorialManager.Instance.topRightPanel.SetActive(true);
                    TutorialManager.Instance.popUps[3].SetActive(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Player") && UIManager.Instance.saveGameButton != null)
                UIManager.Instance.saveGameButton.gameObject.SetActive(false);
        }

        public void ChangeSprite()
        {
            if (checkPointID == InGameManager.Instance.currentCheckPointID)
                StartCoroutine(SaveGameEffect());
            else
                glow_sprite.SetActive(false);
        }

        IEnumerator SaveGameEffect()
        {
            AudioManager.Instance.Play("SaveGameEffect");

            GameObject effect = EffectManager.Instance.Take(EffectType.SaveGameEffect);
            effect.transform.position = new Vector3(InGameManager.Instance.currentCheckPointPos.x, InGameManager.Instance.currentCheckPointPos.y - 0.5f, 0);
            yield return new WaitForSeconds(0.3f);
            glow_sprite.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            EffectManager.Instance.Return(effect);
        }
    }
}

