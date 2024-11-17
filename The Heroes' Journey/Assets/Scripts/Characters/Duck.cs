using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Duck : BaseMainCharacter
    {
        private DialogueTrigger targetDialogueTrigger;
        public override void ActiveSkill()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);

            if (colliders.Length > 1)
            {
                foreach (Collider2D target in colliders)
                {
                    targetDialogueTrigger = target.GetComponent<DialogueTrigger>();
                    if (target.gameObject.CompareTag("Fairy"))
                    {
                        if (!TutorialManager.Instance.talkedWithFairy)
                        {
                            TutorialManager.Instance.talkedWithFairy = true;
                            target.gameObject.GetComponent<Animator>().SetBool("isFlying", true);
                            targetDialogueTrigger.TriggerDialogue("Fairy - Give task");
                            target.gameObject.GetComponent<Animator>().SetBool("isFlying", false);
                        }
                        else
                        {
                            target.gameObject.GetComponent<Animator>().SetBool("isFlying", true);

                            if (!TutorialManager.Instance.hotPotFound)
                            {
                                targetDialogueTrigger.TriggerDialogue("Fairy - Repeat task");
                                target.gameObject.GetComponent<Animator>().SetBool("isFlying", false);
                            }
                            else if (TutorialManager.Instance.hotPotFound && !InGameManager.Instance.isAllSoldiersDefeated)
                            {
                                targetDialogueTrigger.TriggerDialogue("Fairy - Thank");
                            }
                            else if (InGameManager.Instance.isAllSoldiersDefeated)
                            {
                                targetDialogueTrigger.TriggerDialogue("Fairy - Explain plot");
                            }
                        }
                    }
                    else if (target.gameObject.CompareTag("MimicGate"))
                    {
                        target.gameObject.GetComponent<Animator>().SetBool("IsActive", true);

                        if (!InGameManager.Instance.isFourObjectsFound && !UIManager.Instance.isTalkedWithMimicGate)
                        {
                            UIManager.Instance.isTalkedWithMimicGate = true;
                            targetDialogueTrigger.TriggerDialogue("MimicGate - First encounter");
                        }
                        else if(!InGameManager.Instance.isFourObjectsFound && UIManager.Instance.isTalkedWithMimicGate)
                        {
                            targetDialogueTrigger.TriggerDialogue("MimicGate - Repeat task");
                        }
                        else if(InGameManager.Instance.isFourObjectsFound)
                        {
                            targetDialogueTrigger.TriggerDialogue("MimicGate - Open");
                        }
                    }
                    else if (target.gameObject.CompareTag("Doonggeuni"))
                    {
                        target.gameObject.GetComponent<Doonggeuni>().Talk();
                        target.gameObject.GetComponent<Animator>().SetBool("isMoving", false);

                        targetDialogueTrigger.TriggerDialogue("Doonggeuni");
                    }
                    else if (target.gameObject.CompareTag("Kitsune"))
                    {
                        target.gameObject.GetComponent<Animator>().SetBool("isBlocking", false);
                        target.gameObject.GetComponent<Animator>().SetBool("isTalking", true);
                        if (!UIManager.Instance.isTalkedWithKitsune && (!InGameManager.Instance.isLanternFound || !InGameManager.Instance.isSpearFound || !InGameManager.Instance.isThunderFound))
                        {
                            UIManager.Instance.isTalkedWithKitsune = true;
                            targetDialogueTrigger.TriggerDialogue("Kitsune - First encounter");
                        }
                        else if(UIManager.Instance.isTalkedWithKitsune && (!InGameManager.Instance.isLanternFound || !InGameManager.Instance.isSpearFound || !InGameManager.Instance.isThunderFound))
                        {
                            targetDialogueTrigger.TriggerDialogue("Kitsune - Repeat task");
                        }
                        else if(InGameManager.Instance.isLanternFound && InGameManager.Instance.isSpearFound && InGameManager.Instance.isThunderFound)
                        {
                            targetDialogueTrigger.TriggerDialogue("Kitsune - Give book");
                        }
                    }
                    else if (target.gameObject.CompareTag("GreenDragon"))
                    {
                        target.gameObject.GetComponent<GreenDragon>().Talk();

                        targetDialogueTrigger.TriggerDialogue("Green Dragon");
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(transform.position, 3);
        }
    }
}
