using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System;
using TheHeroesJourney;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public Sound[] sounds;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }
        else
        {
            Debug.Log("Found more than one Audio Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
    }

    public void PlayJumpKingSmackSound()
    {
        Play("JumpKingSmack");
    }
}
