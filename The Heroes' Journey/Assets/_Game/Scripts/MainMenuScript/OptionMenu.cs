using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TheHeroesJourney
{
    public class OptionMenu : MonoBehaviour, IDataPresistence
    {
        public static OptionMenu _instance;

        [SerializeField] private AudioMixer musicAudioMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;        
        public static OptionMenu Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<OptionMenu>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Debug.Log("Found more than one Option Menu in the scene. Destroying the newest one");
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            SetMusic();
            SetSfx();
        }

        public void SetMusic()
        {
            float volume = musicSlider.value;
            musicAudioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        }

        public void SetSfx()
        {
            float volume = sfxSlider.value;
            musicAudioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        }

        public void LoadData(GameData data)
        {
            musicSlider.value = data.musicVolume;
            sfxSlider.value = data.sfxVolume;
        }

        public void SaveData(GameData data)
        {
            data.musicVolume = musicSlider.value;
            data.sfxVolume = sfxSlider.value;
        }
    }
}



