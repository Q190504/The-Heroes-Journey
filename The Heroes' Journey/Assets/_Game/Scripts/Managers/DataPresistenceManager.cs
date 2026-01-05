using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace TheHeroesJourney
{
    public class DataPresistenceManager : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool initializeDataIfNull = false;

        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private static DataPresistenceManager _instance;
        private GameData _gameData;

        private List<IDataPresistence> _dataPresistenceObjects;
        private FileDataHandler _dataHandler;

        public static DataPresistenceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DataPresistenceManager>();
                return _instance;
            }

            private set { _instance = value; }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                this._dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.Log("Found more than one Data Presistence Manager in the scene. Destroying the newest one");
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded called");
            this._dataPresistenceObjects = FindAllDataPresistenceObjects();
            LoadGame();
        }

        public void DeleteGameData()
        {
            //delete the data
            _dataHandler.Delete();
        }

        public void NewGame()
        {
            this._gameData = new GameData();
        }

        public void LoadGame()
        {
            //Load any saved data from a file using the data handler
            this._gameData = _dataHandler.Load();

            //start a new game if the data is null and we're configured to initialize data for debugging purposes
            if(_gameData == null && initializeDataIfNull)
            {
                NewGame();
            }

            //if no data can be loaded, initialize a new game
            if(this._gameData == null)
            {
                Debug.Log("No data was found. Initializing data to default");
                NewGame();
            }
            else if(this._gameData.isGameFinished)
            {
                Debug.Log("Had already finished the game. Load new game");
                DeleteGameData();
                NewGame();
            }

            //Push the Loaded data to all other scripts that need it
            foreach (IDataPresistence dataPresistenceObj in _dataPresistenceObjects)
            {
                dataPresistenceObj.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            UIManager.Instance.ShowSaveGameIcon();
            //pass the data to other scripts so thay can update it
            foreach (IDataPresistence dataPresistenceObj in _dataPresistenceObjects)
            {
                dataPresistenceObj.SaveData(_gameData);
            }

            //save that data to a file using the data handler
            _dataHandler.Save(_gameData);
        }

        private List<IDataPresistence> FindAllDataPresistenceObjects()
        {
            IEnumerable<IDataPresistence> dataPresistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPresistence>();

            return new List<IDataPresistence>(dataPresistenceObjects);
        }
    }
}
