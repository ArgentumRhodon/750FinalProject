using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme", 0.25f);
    }

    public void PlayMusic(string name, float volume = 0.25f)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) UnityEngine.Debug.Log("Sound not found!");
        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, float volume = 0.25f)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) UnityEngine.Debug.Log("Sound not found!");
        else
        {
            sfxSource.PlayOneShot(s.clip, volume);
        }
    }
}
