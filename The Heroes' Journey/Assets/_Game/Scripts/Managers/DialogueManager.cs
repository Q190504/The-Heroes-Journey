using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using Cinemachine;

namespace TheHeroesJourney
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        public List<GameObject> characterIcon;
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI dialogueArea;

        private Queue<DialogueLine> lines;

        public bool isDialogueActive = false;

        [Header("UIs")]
        public Animator animator;
        public List<GameObject> UIElementsToToggle;

        [Header("Audio")]
        private int currentVoiceIndex = 0;
        public AudioSource audioSource;
        public AudioClip[] fairyAudioClips;
        public AudioClip[] knightAudioClips;
        public AudioClip[] mimicGateAudioClips;
        public AudioClip[] doonggeuniAudioClips;
        public AudioClip[] kitsuneAudioClips;
        public AudioClip[] greenDragonAudioClips;

        private Dialogue currentDialogue;
        private DialogueLine currentLine;
        private bool isShowingFirstLine = true;
        private bool isLastLineShownFast = false;
        private bool isLastLineShown = false;
        private bool isShowedEntireLine = false;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            lines = new Queue<DialogueLine>();

            audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            isDialogueActive = true;
            foreach (GameObject UIElement in UIElementsToToggle)
                UIElement.SetActive(false);

            animator.Play("DialogueBox_Show");

            lines.Clear();

            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }

        public void DisplayNextDialogueLine()
        {
            if ((isLastLineShownFast || isLastLineShown) && lines.Count == 0)
            {
                EndDialogue();
                return;
            }
            else if (!isShowingFirstLine && dialogueArea.text != currentLine.line.ToString())
            {
                if (lines.Count == 0)
                    isLastLineShownFast = true;

                isShowedEntireLine = true;
                dialogueArea.text = "";
                dialogueArea.text = currentLine.line.ToString();
            }
            else
            {
                foreach (GameObject icon in characterIcon)
                    icon.SetActive(false);
                isShowingFirstLine = false;
                currentLine = lines.Dequeue();

                currentLine.character.icon.SetActive(true);
                characterName.text = currentLine.character.name;
                if (currentLine.character.name == "Fairy")
                {
                    audioSource.PlayOneShot(fairyAudioClips[currentVoiceIndex++ % fairyAudioClips.Length]);
                }
                else if (currentLine.character.name == "Evil King's Soldier 1" || currentLine.character.name == "Evil King's Soldier 2" || currentLine.character.name == "Evil King's Soldier 3")
                {
                    audioSource.PlayOneShot(knightAudioClips[currentVoiceIndex++ % knightAudioClips.Length]);
                }
                else if (currentLine.character.name == "Mimic Gate")
                {
                    audioSource.PlayOneShot(mimicGateAudioClips[currentVoiceIndex++ % mimicGateAudioClips.Length]);
                }
                else if(currentLine.character.name == "Doonggeuni")
                {
                    audioSource.PlayOneShot(doonggeuniAudioClips[currentVoiceIndex++ % doonggeuniAudioClips.Length]);
                }
                else if(currentLine.character.name == "Kitsune")
                {
                    audioSource.PlayOneShot(kitsuneAudioClips[currentVoiceIndex++ % kitsuneAudioClips.Length]);
                }
                else if(currentLine.character.name == "Green Dragon")
                {
                    audioSource.PlayOneShot(greenDragonAudioClips[currentVoiceIndex++ % greenDragonAudioClips.Length]);
                }

                StopAllCoroutines();

                StartCoroutine(TypeSentence(currentLine));
            }
        }

        IEnumerator TypeSentence(DialogueLine dialogueLine)
        {
            dialogueArea.text = "";

            for (int i = 0; i < dialogueLine.line.Length; i++)
            {
                if (isShowedEntireLine)
                {
                    isShowedEntireLine = false;
                    yield break;
                }

                dialogueArea.text += dialogueLine.line[i];
                yield return new WaitForSeconds(dialogueLine.typingDelay);
                if (lines.Count == 0 && i == dialogueLine.line.Length - 1)
                    isLastLineShown = true;
            }
        }

        void EndDialogue()
        {
            currentVoiceIndex = 0;
            isShowedEntireLine = false;
            isShowingFirstLine = true;
            isLastLineShownFast = false;
            isDialogueActive = false;
            animator.Play("DialogueBox_Hide");

            if (currentDialogue.name != "Soldiers - Kiddnap Doonggeuni")
                foreach (GameObject UIElement in UIElementsToToggle)
                    UIElement.SetActive(true);

            if(currentDialogue.name == "Fairy - Give task")
            {
                UIManager.Instance.taskText.text = "Find a hot pot for the fairy!";
                TutorialManager.Instance.popUps[6].SetActive(true);
                FirebaseManager.LogEvent("Event_Fairy_Give_task");
            }
            else if (currentDialogue.name == "Fairy - Thank")
            {
                UIManager.Instance.taskText.text = "Defeat all the evil soldiers!";
                TutorialManager.Instance.isTutorialFinished = true;
                UIManager.Instance.fairy.gameObject.GetComponentInChildren<Animator>().SetBool("isFlying", false);
                UIManager.Instance.fairy.GetComponentInChildren<Fairy>().attackAtMarketCutscene.Play();
                FirebaseManager.LogEvent("Event_AttackAtMarketCutscene");
            }
            else if (currentDialogue.name == "Soldiers - Kiddnap Doonggeuni")
            {
                UIManager.Instance.taskText.text = "Talk to the fairy!";
                InGameManager.Instance.StartSoldierLeavingCutScene();
            }
            else if (currentDialogue.name == "Fairy - Explain plot")
            {
                UIManager.Instance.taskText.text = "Find 4 legendary items!";
                InGameManager.Instance.blockAtMarket.SetActive(false);
                UIManager.Instance.fairy.GetComponentInChildren<Fairy>().FairyDissapear();
                InGameManager.Instance.currentCheckPointPos = InGameManager.Instance.player.transform.position;
                InGameManager.Instance.ChangeCameraBoundary(InGameManager.Instance.fullCameraBoundary);
                DataPresistenceManager.Instance.SaveGame();
                FirebaseManager.LogEvent("Event_Fairy_Explain_plot");
            }
            else if (currentDialogue.name == "MimicGate - First encounter")
            {
                UIManager.Instance.mimicGate.gameObject.GetComponent<Animator>().SetBool("IsActive", false);
                UIManager.Instance.ShowFourObjPlatform();
                FirebaseManager.LogEvent("Event_MimicGate_First_encounter");
            }
            else if (currentDialogue.name == "MimicGate - Repeat task")
                UIManager.Instance.mimicGate.gameObject.GetComponent<Animator>().SetBool("IsActive", false);
            else if (currentDialogue.name == "MimicGate - Open")
            {
                UIManager.Instance.mimicGate.gameObject.GetComponent<Animator>().SetBool("IsActive", false);
                AudioManager.Instance.Play("MimicGateOpen");
                UIManager.Instance.taskText.text = "Defeat the evil king!";
                TransitionManager.Instance.StartCrossfadeTrasition();
                StartCoroutine(DelayEnterJumpKingArena());
                FirebaseManager.LogEvent("Event_MimicGate_Open");
            }
            else if (currentDialogue.name == "Green Dragon")
            {
                UIManager.Instance.greenDragon.GetComponentInChildren<GreenDragon>().NPCDisappear();
                UIManager.Instance.DisableAllAnimalUnderGround();
                FirebaseManager.LogEvent("Event_GreenDragon_Dissapear");
            }
            else if (currentDialogue.name == "Doonggeuni")
            {
                AudioManager.Instance.Play("CollectItem");
                UIManager.Instance.foundDoonggeuniPanel.SetActive(true);
                UIManager.Instance.doonggeuni.gameObject.SetActive(false);
                UIManager.Instance.doonggeuniCellWall.SetActive(false);
                FirebaseManager.LogEvent("Event_Found_Doonggeuni");
            }
            else if (currentDialogue.name == "Kitsune - First encounter")
            {
                UIManager.Instance.kitsune.gameObject.GetComponent<Animator>().SetBool("isTalking", false);
                FirebaseManager.LogEvent("Event_Kitsune_FirstMet");
            }
            else if (currentDialogue.name == "Kitsune - Repeat task")
            {
                UIManager.Instance.kitsune.gameObject.GetComponent<Animator>().SetBool("isTalking", false);
            }
            else if (currentDialogue.name == "Kitsune - Give book")
            {
                UIManager.Instance.kitsune.gameObject.GetComponent<Animator>().SetBool("isTalking", false);
                UIManager.Instance.kitsune.gameObject.GetComponent<NPC>().NPCDisappear();
                UIManager.Instance.book.SetActive(true);
                FirebaseManager.LogEvent("Event_Kitsune_Dissapear");
            }
        }

        IEnumerator DelayEnterJumpKingArena()
        {
            yield return new WaitForSeconds(1);
            InGameManager.Instance.EnterJumpKingFight();
        }
    }
}

