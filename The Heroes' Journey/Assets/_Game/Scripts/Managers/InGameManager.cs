using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;
using Cinemachine;

namespace TheHeroesJourney
{
    public class InGameManager : MonoBehaviour, IDataPresistence
    {
        public static InGameManager Instance { get; private set; }

        public Player player;
        
        [SerializeField] private bool spawnAtStartPos = false;
        public GameObject startPos;
        public int currentCheckPointID;
        public Vector2 currentCheckPointPos;
        public Vector2 respawnPos;
        public UnityAction<float> _onSaveGame;

        public GameObject tutorialDoonggeuni;
        public GameObject tutorialDoonggeuniTeleportPosition;
        private bool isMarketCutScenePlayed;
        public bool isAllSoldiersDefeated;
        public List<GameObject> soldiers;

        public PlayableDirector soldiersArrivingCutScence;
        public DialogueTrigger soldierDialogue;
        public PlayableDirector soldiersLeavingCutScence;
        public GameObject blockAtMarket;

        public Collider2D tutorialCameraBoundary;
        public Collider2D fullCameraBoundary;
        public bool isCameraBoundaryChanged;
        public CinemachineVirtualCamera boundedCamera;
        public CinemachineVirtualCamera staticCameraForSoldiersArriving;

        public bool isPolarBearFound;
        public UnityAction _onPolarBearFound;

        public bool isTicketFound;
        public bool isSpearFound;
        public bool isThunderFound;
        public bool isLanternFound;
        public bool isBookFound;
        public bool isFourObjectsFound;

        public bool isBattleCultisPriest;

        public PlayableDirector enterjumpKingArena;
        public GameObject arenaEntrance;
        public bool isBattleJumpKing;
        public bool isGameFinished;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this.gameObject);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            AudioManager.Instance.Stop("MainMenuTheme");
            AudioManager.Instance.Play("MainGameTheme");

            foreach (GameObject soldier in soldiers)
                soldier.SetActive(false);

            isBattleCultisPriest = false;

            if (spawnAtStartPos)
            {
                respawnPos = startPos.transform.position;
                player.gameObject.transform.position = startPos.transform.position;
            }

            if (!isCameraBoundaryChanged)
                ChangeCameraBoundary(tutorialCameraBoundary);
            else
                ChangeCameraBoundary(fullCameraBoundary);
        }

        // Update is called once per frame
        void Update()
        {
            if (!isAllSoldiersDefeated && isMarketCutScenePlayed)
            {
                foreach (GameObject soldier in soldiers)
                    if (soldier.activeSelf)
                        return;
                isAllSoldiersDefeated = true;
                soldiersArrivingCutScence.Play();
            }
        }



        public void PolarBearFound()
        {
            FirebaseManager.LogEvent("PolarBear_Found");
            isPolarBearFound = true;
            _onPolarBearFound?.Invoke();
        }

        public void EnterJumpKingFight()
        {
            respawnPos = arenaEntrance.transform.position;
            player.gameObject.transform.position = arenaEntrance.transform.position;
            isBattleJumpKing = true;
            LightManager.Instance.TurnOnPlayerLight();

            //SAVE GAME
            DataPresistenceManager.Instance.SaveGame();

            UIManager.Instance.jumpKing.gameObject.SetActive(true);
            UIManager.Instance.jumpKing.SetDefaultStats();
            if (isPolarBearFound)
                player.GetComponent<PolarBear>().enabled = true;

            AudioManager.Instance.Stop("MainGameTheme");
            AudioManager.Instance.Play("BattleJumpKingTheme");
            StartCoroutine(StartCutscene());
        }

        IEnumerator StartCutscene()
        {
            yield return new WaitForSeconds(2);
            enterjumpKingArena.Play();
        }

        public void EnterAttackAtMarketCutScene()
        {
            UIManager.Instance.ToggleUIForCutScence();
            foreach (GameObject soldier in soldiers)
                soldier.SetActive(true);
        }

        public void ExitAttackAtMarketCutScene()
        {
            isMarketCutScenePlayed = true;
            UIManager.Instance.ToggleUIForCutScence();
        }

        public void StartSoldierArrivingCutscence()
        {
            CameraManager.Instance.SwapCamera(boundedCamera, staticCameraForSoldiersArriving, new Vector2(1, 0));
        }

        public void StartSoldierDialogue()
        {
            soldierDialogue.TriggerDialogue("Soldiers - Kiddnap Doonggeuni");
        }

        public void StartSoldierLeavingCutScene()
        {
            soldiersLeavingCutScence.Play();
        }

        public void ExitSoldierLeavingCutScene()
        {
            CameraManager.Instance.SwapCamera(boundedCamera, staticCameraForSoldiersArriving, new Vector2(-1, 0));

            //SAVE GAME
            respawnPos = player.transform.position;
            DataPresistenceManager.Instance.SaveGame();
            FirebaseManager.LogEvent("Event_ExitSoldierLeavingCutScene");
        }

        public void ChangeCameraBoundary(Collider2D boundary)
        {
            isCameraBoundaryChanged = true;
            boundedCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = boundary;
        }

        public void LoadData(GameData data)
        {
            //Player's info
            if (data.playerPosition == Vector2.zero)
            {
                player.gameObject.transform.position = startPos.transform.position;
                respawnPos = player.gameObject.transform.position = startPos.transform.position;
            }
             else
                player.gameObject.transform.position = data.playerPosition;

            respawnPos = data.respawnPosition;


            //Game's progress
            tutorialDoonggeuni.SetActive(data.tutorialDoonggeuniActiveState);
            this.isMarketCutScenePlayed = data.isMarketCutScenePlayed;
            this.isAllSoldiersDefeated = data.isAllSoldiersDefeated;
            this.isCameraBoundaryChanged = data.isCameraBoundaryChanged;
            this.blockAtMarket.SetActive(data.blockAtMarketActiveState);

            this.isPolarBearFound = data.isPolarBearFound;
            this.isSpearFound = data.isSpearFound;
            this.isThunderFound = data.isThunderFound;
            this.isLanternFound = data.isLanternFound;
            this.isBookFound = data.isBookFound;
            this.isFourObjectsFound = data.isFourObjectsFound;

            this.isBattleJumpKing = data.isBattleJumpKing;
            this.isGameFinished = data.isGameFinished;
            if (data.isBattleJumpKing)
                EnterJumpKingFight();
        }

        public void SaveData(GameData data)
        {
            if (currentCheckPointID == 0)
                TutorialManager.Instance.pressSaveGame = true;
            _onSaveGame?.Invoke(player.playerConfig.maxHealth);

            //Player's info
            respawnPos = currentCheckPointPos;
            data.playerPosition = currentCheckPointPos;
            data.respawnPosition = respawnPos;
            data.isCameraBoundaryChanged = this.isCameraBoundaryChanged;

            //Game's progress
            data.tutorialDoonggeuniActiveState = tutorialDoonggeuni.activeSelf;
            data.isMarketCutScenePlayed = this.isMarketCutScenePlayed;
            data.isAllSoldiersDefeated = this.isAllSoldiersDefeated;
            data.blockAtMarketActiveState = blockAtMarket.activeSelf;

            data.isTicketFound = this.isTicketFound;
            data.isPolarBearFound = this.isPolarBearFound;
            data.isSpearFound = this.isSpearFound;
            data.isThunderFound = this.isThunderFound;
            data.isLanternFound = this.isLanternFound;
            data.isBookFound = this.isBookFound;
            data.isFourObjectsFound = this.isFourObjectsFound;
            data.isBattleJumpKing = this.isBattleJumpKing;
            data.isGameFinished = this.isGameFinished;
        }
    }
}
