using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;

namespace TheHeroesJourney
{
    public class UIManager : MonoBehaviour, IDataPresistence
    {
        public static UIManager Instance { get; private set; }

        [Header("UI")]
        public TMP_Text healthStatus;
        public List<Button> switchCharacterButtonsList;
        public Button skillButton;
        public Image skillCooldownFillImage;
        public Image basicAttackCooldownFillImage;
        public List<Sprite> skillImageList;
        public Image meleeAttackCooldownFillImage;
        public List<GameObject> characterProfileList;

        public List<GameObject> UIElementsWhenToggleMap;
        public GameObject mapHandler;
        public GameObject currentMap;
        public List<GameObject> mapList;
        private bool isMapOpened;

        public List<GameObject> UIElementsToToggleWhenCutSceces;

        public Button saveGameButton;
        public Image saveGameImage;

        public Button openTaskButton;

        [Header("Panels")]
        public GameObject foundHotpotPanel;
        public GameObject foundJejuTicketPanel;
        public GameObject foundDoonggeuniPanel;

        public GameObject foundSpearPanel;
        public GameObject foundLanternPanel;
        public GameObject foundThunderPanel;
        public GameObject foundBookPanel;
        public GameObject foundPolarBearPanel;
        public GameObject polarBearProfileButton;

        public GameObject taskPanel;
        public TMP_Text taskText;

        public GameObject pauseMenuPanel;
        public GameObject optionPanel;
        public GameObject creditPanel;

        public GameObject showErrorPanel;
        public TMP_Text errorDetail;

        [Header("GameDisplay")]
        public List<CheckPoint> checkPointList;

        public List<GameObject> villagersAtMarket;
        public GameObject brigde;

        public List<GameObject> objPlatformList;
        public List<GameObject> displayObjList;

        public GameObject thunder;
        public GameObject book;
        public GameObject spear;
        public GameObject lantern;

        public GameObject fairy;
        public GameObject fairyDefaultPosition;

        public NPC mimicGate;
        public bool isTalkedWithMimicGate;

        public GameObject kitsune;
        public bool isTalkedWithKitsune;

        public GameObject greenDragon;
        public List<GameObject> animalsUndergroundList;

        public GameObject doonggeuni;
        public GameObject doonggeuniCellWall;

        public GameObject cultistPriestlEntranceWall;

        public GameObject cultisPriest;

        public JumpKing jumpKing;
        public List<GameObject> benchCharacterList;
        public List<GameObject> popUpFightEmotes;
        public List<GameObject> popUpCelebrateEmotes;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            PauseManager.Instance.onChangePause += TogglePauseMenu;
            InGameManager.Instance.player._onChangeHealth += UpdateHealhStatus;
            InGameManager.Instance.player._onChangeCharacter += UpdateSkillButton;
            InGameManager.Instance.player._onCooldownSkill += UpdateCooldownSkillButton;
            InGameManager.Instance.player._onChangeCharacter += UpdateSwitchCharacterButtons;
            InGameManager.Instance.player._onChangeCharacter += UpdateBenchCharacters;
            InGameManager.Instance._onPolarBearFound += OnChangePolarBearFound;
            InGameManager.Instance._onSaveGame += ChangeCheckPointSprite;
            InGameManager.Instance.player._onDogHealing += ToggleSwitchCharaterButtons;

            UpdateSwitchCharacterButtons(InGameManager.Instance.player._currentCharacterIndex);
            UpdateBenchCharacters(InGameManager.Instance.player._currentCharacterIndex);

            saveGameButton.gameObject.SetActive(false);
            saveGameImage.gameObject.SetActive(false);

            isMapOpened = false;
            mapHandler.SetActive(false);
            pauseMenuPanel.SetActive(false);
            showErrorPanel.SetActive(false);

            fairy.gameObject.transform.position = fairyDefaultPosition.transform.position;

            cultistPriestlEntranceWall.SetActive(false);

            foreach (GameObject emote in popUpFightEmotes)
                emote.SetActive(true);

            foreach (GameObject emote in popUpCelebrateEmotes)
                emote.SetActive(false);


            optionPanel.SetActive(true);
            optionPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void TogglePauseMenu(bool isPause)
        {
            pauseMenuPanel.SetActive(isPause);
        }

        public void UpdateHealhStatus(float currentHealth, float maxHealth)
        {
            if (currentHealth >= maxHealth * 0.8)
                healthStatus.color = Color.green;
            else if (currentHealth < maxHealth * 0.8 && currentHealth > maxHealth * 0.2)
                healthStatus.color = new Color(142f / 255f, 142f / 255f, 0f, 255f);
            else if (currentHealth <= maxHealth * 0.2)
                healthStatus.color = Color.red;
            healthStatus.text = $"{currentHealth}/{maxHealth}";
        }

        public void TurnOffSkillButton()
        {
            skillButton.interactable = false;
        }

        public void TurnOnSkillButton()
        {
            skillButton.interactable = true;
        }

        public void HideSkillButton()
        {
            skillButton.gameObject.SetActive(false);
        }

        public void ShowSkillButton()
        {
            skillButton.gameObject.SetActive(true);
        }

        private void UpdateSkillButton(int currentCharacterIndex)
        {
            for (int i = 0; i < skillImageList.Count; i++)
            {
                if (i == currentCharacterIndex)
                {
                    Image buttonImage = skillButton.image;

                    if (buttonImage != null)
                    {
                        buttonImage.sprite = skillImageList[currentCharacterIndex];
                        return;
                    }
                }
            }
        }

        public void UpdateCooldownSkillButton(float fillAmount)
        {
            skillCooldownFillImage.fillAmount = fillAmount;
        }

        public void UpdateCooldownBasicAttackButton(float fillAmount)
        {
            basicAttackCooldownFillImage.fillAmount = fillAmount;
        }

        private void UpdateSwitchCharacterButtons(int currentCharacterIndex)
        {
            for (int i = 0; i < switchCharacterButtonsList.Count; i++)
            {
                if (i == currentCharacterIndex)
                    switchCharacterButtonsList[i].interactable = false;
                else
                    switchCharacterButtonsList[i].interactable = true;
            }
        }

        private void ChangeCheckPointSprite(float notUsedParameter)
        {
            foreach (CheckPoint checkpoint in checkPointList)
                checkpoint.ChangeSprite();
        }

        public void OnChangePolarBearFound()
        {
            foundJejuTicketPanel.SetActive(false);
            AudioManager.Instance.Play("CollectItem");
            foundPolarBearPanel.SetActive(true);
            benchCharacterList[benchCharacterList.Count - 1].SetActive(true);
            polarBearProfileButton.SetActive(true);
            InGameManager.Instance.respawnPos = InGameManager.Instance.player.transform.position;
            DataPresistenceManager.Instance.SaveGame();
        }

        public void OpenOptionPanel()
        {
            AudioManager.Instance.Play("Click");
            optionPanel.gameObject.SetActive(true);
        }

        public void CloseOptionPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            optionPanel.gameObject.SetActive(false);
        }

        public void OpenTaskPanel()
        {
            AudioManager.Instance.Play("Click");
            taskPanel.gameObject.SetActive(true);
        }

        public void CloseTaskPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            taskPanel.gameObject.SetActive(false);
        }


        public void CloseFoundPolarBearPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundPolarBearPanel.SetActive(false);
        }
        public void CloseTicketPanel()
        {
            InGameManager.Instance.respawnPos = InGameManager.Instance.player.transform.position;
            DataPresistenceManager.Instance.SaveGame();
            AudioManager.Instance.Play("ShortClick");
            foundJejuTicketPanel.SetActive(false);
        }
        public void CloseFoundDoonggeuniPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundDoonggeuniPanel.SetActive(false);
        }
        public void CloseFoundSpearPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundSpearPanel.SetActive(false);
        }
        public void CloseFoundLanternPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundLanternPanel.SetActive(false);
        }
        public void CloseFoundThunderPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundThunderPanel.SetActive(false);
        }
        public void CloseFoundHotpotPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundHotpotPanel.SetActive(false);
        }
        public void CloseFoundBookPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            foundBookPanel.SetActive(false);
        }

        public void OpenDuckProfilePanel()
        {
            AudioManager.Instance.Play("Click");
            characterProfileList[0].SetActive(true);
        }
        public void OpenTigerProfilePanel()
        {
            AudioManager.Instance.Play("Click");
            characterProfileList[1].SetActive(true);
        }
        public void OpenPenguinProfilePanel()
        {
            AudioManager.Instance.Play("Click");
            characterProfileList[2].SetActive(true);
        }
        public void OpenCatProfilePanel()
        {
            AudioManager.Instance.Play("Click");
            characterProfileList[3].SetActive(true);
        }
        public void OpenDogProfilePanel()
        {
            AudioManager.Instance.Play("Click");

            characterProfileList[4].SetActive(true);
        }
        public void OpenPolarBearProfilePanel()
        {
            AudioManager.Instance.Play("Click");

            characterProfileList[5].SetActive(true);
        }
        public void CloseDuckProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[0].SetActive(false);
        }
        public void CloseTigerProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[1].SetActive(false);
        }
        public void ClosePenguinProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[2].SetActive(false);
        }
        public void CloseCatProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[3].SetActive(false);
        }
        public void CloseDogProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[4].SetActive(false);
        }
        public void ClosePolarBearProfilePanel()
        {
            AudioManager.Instance.Play("ShortClick");

            characterProfileList[5].SetActive(false);
        }

        public void ShowErrorPanel(string error)
        {
            InGameManager.Instance.player.GetComponent<Move>().SetDefaultValueMoveButtons();
            InGameManager.Instance.player.GetComponent<Jump>().SetDefaultValueJumpButtons();
            AudioManager.Instance.Play("ErrorPanel");
            showErrorPanel.SetActive(true);
            errorDetail.text = error;
        }

        public void CloseShowErrorPanel()
        {
            AudioManager.Instance.Play("ShortClick");
            showErrorPanel.SetActive(false);
        }

        public void DisableAllAnimalUnderGround()
        {
            foreach (GameObject animal in animalsUndergroundList)
                animal.SetActive(false);
        }

        public void UpdateBenchCharacters(int currentCharacterIndex)
        {
            for (int i = 0; i < benchCharacterList.Count; i++)
                benchCharacterList[i].SetActive(currentCharacterIndex != i);
        }

        public void ShowFourObjPlatform()
        {
            foreach (GameObject platform in objPlatformList)
            {
                platform.SetActive(true);
                StartCoroutine(SlowlyShow(platform, 0f, 1.0f, 0.5f)); // Thời gian hiện: 0.5 giây
            }
        }

        private IEnumerator SlowlyShow(GameObject platform ,float startAlpha, float targetAlpha, float duration)
        {
            SpriteRenderer renderer = platform.GetComponent<SpriteRenderer>();

            float currentTime = 0.0f;
            Color startColor = renderer.material.color;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;

                // Lerp giữa startAlpha và targetAlpha theo thời gian
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);

                // Đặt alpha vào màu của vật thể
                Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
                renderer.material.color = newColor;

                yield return null;
            }

            // Đảm bảo đặt màu cuối cùng
            renderer.material.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        }

        public void SetActiveCultistPriestlEntranceWall(bool status)
        {
            cultistPriestlEntranceWall.SetActive(status);
        }

        public void ToggleSwitchCharaterButtons(bool state)
        {
            for(int i = 0; i < switchCharacterButtonsList.Count - 1; i++)
                switchCharacterButtonsList[i].interactable = state;
        }

        public void ToggleMap()
        {
            if (isMapOpened)
            {
                //Close
                AudioManager.Instance.Play("ShortClick");

                isMapOpened = false;
                mapHandler.SetActive(false);
                currentMap.SetActive(false);

                if(!DialogueManager.Instance.isDialogueActive && TutorialManager.Instance.GetCurrentPopUpIndex() >= 6)
                    foreach (var item in UIElementsWhenToggleMap)
                        item.SetActive(true);
            }
            else
            {
                //Open
                InGameManager.Instance.player.GetComponent<Move>().SetDefaultValueMoveButtons();
                InGameManager.Instance.player.GetComponent<Jump>().SetDefaultValueJumpButtons();

                AudioManager.Instance.Play("Click");

                isMapOpened = true;
                mapHandler.SetActive(true);
                currentMap.SetActive(true);

                if (!DialogueManager.Instance.isDialogueActive && TutorialManager.Instance.GetCurrentPopUpIndex() >= 6)
                    foreach (var item in UIElementsWhenToggleMap)
                        item.SetActive(false);
            }
        }

        public void SetCurrentMap(string mapName)
        {
            if (currentMap != null && currentMap.name == mapName)
                return;

            foreach (GameObject map in mapList)
            {
                if (map.name == mapName)
                {
                    currentMap = map;
                    return;
                }
            }
        }

        public void ToggleUIForCutScence()
        {
            foreach (GameObject UIElement in UIElementsToToggleWhenCutSceces)
                UIElement.SetActive(!UIElement.activeSelf);
        }

        public void DissappearFairy()
        {
            fairy.gameObject.SetActive(false);
        }

        public void FairyReappear()
        {
            fairy.gameObject.SetActive(true);
            fairy.transform.position = new Vector3(InGameManager.Instance.player.transform.position.x - 1, fairy.transform.position.y, fairy.transform.position.z);
        }

        public void EndingGameDisplay()
        {
            AudioManager.Instance.Play("CrowCheer");


            foreach (GameObject emote in popUpFightEmotes)
                emote.SetActive(false);

            foreach (GameObject emote in popUpCelebrateEmotes)
                emote.SetActive(true);
        }

        public void ShowGameCredit()
        {
            AudioManager.Instance.Stop("BattleJumpKingTheme");
            AudioManager.Instance.Play("CreditTheme");

            creditPanel.GetComponent<Animator>().SetTrigger("start");
            FirebaseManager.LogEvent("Event_FinishGame_ShowGameCredit");
        }

        public void ShowSaveGameIcon()
        {
            StartCoroutine(SaveGameIconAnimation());
        }

        IEnumerator SaveGameIconAnimation()
        {
            saveGameImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            saveGameImage.gameObject.SetActive(false);
        }

        public void LoadData(GameData data)
        {
            //UI
            SetCurrentMap(data.currentMapName);
            polarBearProfileButton.SetActive(data.isPolarBearFound);
            taskText.text = data.currentTask;

            //Game objs
            fairy.gameObject.SetActive(data.fairyActiveState);
            foreach (GameObject villager in villagersAtMarket)
                villager.SetActive(!data.isMarketCutScenePlayed);
            brigde.SetActive(data.brigdeActiveState);

            isTalkedWithMimicGate = data.isTalkedWithMimicGate;
            foreach (GameObject platform in objPlatformList)
                platform.SetActive(data.isTalkedWithMimicGate);

            isTalkedWithKitsune = data.isTalkedWithKitsune;
            kitsune.SetActive(data.kitsuneActiveState);
            doonggeuni.gameObject.SetActive(data.doonggeuniActiveState);
            doonggeuniCellWall.SetActive(data.doonggeuniActiveState);
            greenDragon.gameObject.SetActive(data.greenDragonActiveState);
            foreach (GameObject animal in animalsUndergroundList)
                animal.SetActive(data.greenDragonActiveState);

            thunder.SetActive(data.isThunderFound);
            displayObjList[0].SetActive(data.isThunderFound);

            spear.SetActive(!data.isSpearFound);
            displayObjList[1].SetActive(data.isSpearFound);

            book.SetActive(data.isLanternFound && data.isSpearFound && data.isThunderFound && !data.kitsuneActiveState);
            displayObjList[2].SetActive(data.isBookFound);

            lantern.SetActive(!data.isLanternFound); 
            displayObjList[3].SetActive(data.isLanternFound);

            jumpKing.gameObject.SetActive(data.isBattleJumpKing);
            for(int i = 0; i < benchCharacterList.Count; i++)
                benchCharacterList[i].SetActive(!(data.currentCharacterIndex == i));

            cultisPriest.SetActive(data.cultisPriestActiveState);
        }

        public void SaveData(GameData data)
        {
            //UI
            data.currentMapName = currentMap.name;
            data.currentTask = taskText.text;

            //Game objs
            data.fairyActiveState = fairy.gameObject.activeSelf;
            data.brigdeActiveState = brigde.activeSelf;

            data.isTalkedWithMimicGate = isTalkedWithMimicGate;
            data.isTalkedWithKitsune = isTalkedWithKitsune;
            data.kitsuneActiveState = kitsune.activeSelf;
            data.doonggeuniActiveState = doonggeuni.gameObject.activeSelf;
            data.greenDragonActiveState = greenDragon.gameObject.activeSelf;

            data.cultisPriestActiveState = cultisPriest.activeSelf;
            data.isBattleJumpKing = jumpKing.gameObject.activeSelf;
        }
    }
}

