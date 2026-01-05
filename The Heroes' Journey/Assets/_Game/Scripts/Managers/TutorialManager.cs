using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class TutorialManager : MonoBehaviour, IDataPresistence
    {
        public static TutorialManager Instance { get; private set; }

        public List<GameObject> popUps;
        public bool isTutorialFinished;

        private int popUpIndex;

        private bool moveButtonPressed;
        private bool jumpButtonPressed;
        public bool pressSaveGame;
        public bool climbWall;
        public bool talkedWithFairy;
        public bool taskButtonPressed;
        private bool basicAttackPressed;
        public bool dogSkillPressed;
        public bool penguinSkillPressed;
        public bool catSkillPressed;
        public bool tigerSkillPressed;
        public bool slideWall;
        public bool jumpWall;
        public bool hotPotFound;
        public bool mapOpened;
        public bool mapClosed;

        public GameObject blockAtSaveGameTutorial;


        public GameObject topRightPanel;
        public GameObject switchCharacterPanel;
        public GameObject bottomRightPanel;
        public GameObject skillButton;
        public GameObject basicAttackButton;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isTutorialFinished) return;

            for (int i = 0; i < popUps.Count; i++)
            {
                if (popUpIndex == 0)
                {
                    if (moveButtonPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 1 || popUpIndex == 2)
                {
                    bottomRightPanel.SetActive(true);
                    if (jumpButtonPressed)
                    {
                        jumpButtonPressed = false;
                        UpdatePopUpPanels();
                    }
                }
                else if (popUpIndex == 3)
                {
                    if (pressSaveGame)
                    {
                        blockAtSaveGameTutorial.SetActive(false);
                        UpdatePopUpPanels();
                    }
                }
                else if (popUpIndex == 4)
                {
                    if (climbWall)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 5)
                {
                    if (talkedWithFairy)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 6)
                {
                    if (taskButtonPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 7)
                {
                    if (mapOpened && mapClosed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 8)
                {
                    basicAttackButton.SetActive(true);
                    if (basicAttackPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 9)
                {
                    if (dogSkillPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 10)
                {
                    if (penguinSkillPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 11)
                {
                    if (catSkillPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 12)
                {
                    if (tigerSkillPressed)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 13)
                {
                    if (slideWall)
                        UpdatePopUpPanels();
                }
                else if (popUpIndex == 14)
                {
                    if (jumpWall)
                        UpdatePopUpPanels();
                }
            }
        }

        public void UpdatePopUpPanels()
        {
            popUps[popUpIndex++].SetActive(false);
            if(popUpIndex == popUps.Count)
            {
                isTutorialFinished = true;
                return;
            }
            else if (popUpIndex < popUps.Count && popUpIndex != 3 && popUpIndex != 5 && popUpIndex != 6 && popUpIndex != 7 && popUpIndex != 8 && popUpIndex != 13 && popUpIndex != 14)
                popUps[popUpIndex].SetActive(true);
        }

        public int GetCurrentPopUpIndex()
        {
            return popUpIndex;
        }

        public void MoveButtonPressed()
        {
            moveButtonPressed = true;
        }
        public void JumpButtonPressed()
        {
            jumpButtonPressed = true;
        }

        public void BasicAttackButtonPressed()
        {
            basicAttackPressed = true;
        }
        public void OpenTaskButtonPressed()
        {
            taskButtonPressed = true;
        }
        public void MapButtonPressed()
        {
            if (!mapOpened && popUpIndex == 7)
                mapOpened = true;
            else if(mapOpened && popUpIndex == 7)
                mapClosed = true;
        }

        public void LoadData(GameData data)
        {
            isTutorialFinished = data.isTutorialFinished;
            if (!data.isTutorialFinished)
            {
                popUpIndex = 0;
                popUps[popUpIndex].SetActive(true);

                moveButtonPressed = false;
                jumpButtonPressed = false;
                climbWall = false;
                pressSaveGame = false;
                blockAtSaveGameTutorial.SetActive(true);
                talkedWithFairy = false;
                taskButtonPressed = false;
                basicAttackPressed = false;
                dogSkillPressed = false;
                penguinSkillPressed = false;
                catSkillPressed = false;
                tigerSkillPressed = false;
                slideWall = false;
                jumpWall = false;
                hotPotFound = false;
                mapOpened = false;
                mapClosed = false;


                //UIs
                topRightPanel.SetActive(false);
                switchCharacterPanel.SetActive(false);
                bottomRightPanel.SetActive(false);
                skillButton.SetActive(false);
                basicAttackButton.SetActive(false);
            }
            else
            {
                popUpIndex = 14;

                moveButtonPressed = true;
                jumpButtonPressed = true;
                climbWall = true;
                pressSaveGame = true;
                blockAtSaveGameTutorial.SetActive(false);
                talkedWithFairy = true;
                taskButtonPressed = true;
                basicAttackPressed = true;
                dogSkillPressed = true;
                penguinSkillPressed = true;
                catSkillPressed = true;
                tigerSkillPressed = true;
                slideWall = true;
                jumpWall = true;
                hotPotFound = true;
                mapOpened = true;
                mapClosed = true;

                //UIs
                topRightPanel.SetActive(true);
                switchCharacterPanel.SetActive(true);
                bottomRightPanel.SetActive(true);
                skillButton.SetActive(true);
                basicAttackButton.SetActive(true);
            }
        }

        public void SaveData(GameData data)
        {
            data.isTutorialFinished = isTutorialFinished;
        }
    }
}

