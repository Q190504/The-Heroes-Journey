using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TheHeroesJourney
{
    public class LightManager : MonoBehaviour, IDataPresistence
    {
        public static LightManager _instance;

        public GameObject playerLight;

        public static LightManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LightManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void TogglePlayerLight()
        {
            playerLight.SetActive(!playerLight.activeSelf);
        }

        public void TurnOnPlayerLight()
        {
            playerLight.SetActive(true);
        }

        public void TurnOffPlayerLight()
        {
            playerLight.SetActive(false);
        }

        public void LoadData(GameData data)
        {
            playerLight.SetActive(data.playerLight);
        }

        public void SaveData(GameData data)
        {
            data.playerLight = playerLight.activeSelf;
        }
    }
}
