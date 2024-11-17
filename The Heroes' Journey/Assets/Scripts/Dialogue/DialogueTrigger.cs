using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

namespace TheHeroesJourney
{
    [System.Serializable]
    public class DialogueCharacter
    {
        public string name;
        public GameObject icon;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter character;
        [TextArea(3, 10)]
        public string line;
        public float typingDelay;
    }

    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    }

    public class DialogueTrigger : MonoBehaviour
    {
        public List<Dialogue> dialogueList;

        public void TriggerDialogue(string name)
        {
            foreach (Dialogue dialogue in dialogueList)
            {
                if (dialogue.name == name)
                {
                    DialogueManager.Instance.StartDialogue(dialogue);
                    return;
                }
            }
            
        }
    }
}

