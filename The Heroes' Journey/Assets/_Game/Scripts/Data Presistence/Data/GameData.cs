using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    [System.Serializable]
    public class GameData
    {
        [Header("Player's Info")]
        public Vector2 playerPosition;
        public Vector2 respawnPosition;
        public int currentCharacterIndex;
        public bool playerLight;
        public bool isCameraBoundaryChanged;

        [Header("UIs's Info")]
        public float musicVolume;
        public float sfxVolume;
        public string currentMapName;
        public string virtualCameraName;
        public string currentTask;

        [Header("Tutorial Progress's Info")]
        public bool isTutorialFinished;
        public bool blockAtMarketActiveState;

        [Header("Progress's Info")]
        public bool tutorialDoonggeuniActiveState;

        public bool isTalkedWithFairy;
        public bool fairyActiveState;

        public bool isMarketCutScenePlayed;
        public bool isAllSoldiersDefeated;
        public bool brigdeActiveState;

        public bool isTalkedWithMimicGate;
        public bool isTalkedWithKitsune;
        public bool kitsuneActiveState;
        public bool greenDragonActiveState;
        public bool doonggeuniActiveState;

        public bool isTicketFound;
        public bool isPolarBearFound;
        public bool isSpearFound;
        public bool isThunderFound;
        public bool isLanternFound;
        public bool isBookFound;
        public bool isFourObjectsFound;

        public bool cultisPriestActiveState;
        public bool isBattleJumpKing;
        public bool isGameFinished;


        //The values defined in this constructor will be the default values
        //the game starts with when there's no data 
        public GameData()
        {
            //Player's Info
            playerPosition = Vector3.zero;
            respawnPosition = Vector3.zero;
            currentCharacterIndex = 0;
            playerLight = false;
            isCameraBoundaryChanged = false;

            //Tutorial Progress's Info
            isTutorialFinished = false;
            blockAtMarketActiveState = true;

            //UIs
            musicVolume = 1;
            sfxVolume = 1;
            currentMapName = "OutsideMap";
            virtualCameraName = "BOUNDEDCenterPlayerFollowCAM(OpenRooms)";
            currentTask = "Go forward!";

            //Game Progress's Info
            tutorialDoonggeuniActiveState = true;
            fairyActiveState = true;
            isTalkedWithFairy = false;
            isMarketCutScenePlayed = false;
            isAllSoldiersDefeated = false;
            brigdeActiveState = false;
            isTalkedWithMimicGate = false;
            isTalkedWithKitsune = false;
            kitsuneActiveState = true;
            greenDragonActiveState = true;
            doonggeuniActiveState = true;
            isTicketFound = false;
            isPolarBearFound = false;
            isSpearFound = false;
            isThunderFound = false;
            isLanternFound = false;
            isBookFound = false;
            isFourObjectsFound = false;
            cultisPriestActiveState = true;
            isBattleJumpKing = false;
            isGameFinished = false;
        }
    }
}

