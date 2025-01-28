using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAudioManager : MonoBehaviour
{
    public static VolumeAudioManager Instance { get; private set; }

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float voiceVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateAllAudioSources();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateAllAudioSources();
    }

    public void SetVoiceVolume(float volume)
    {
        voiceVolume = volume;
        UpdateAllAudioSources();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateAllAudioSources();
    }

    private void UpdateAllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource source in allAudioSources)
        {
            if (source.gameObject.CompareTag("Music"))
            {
                source.volume = musicVolume * masterVolume;
            }
            else if (source.gameObject.CompareTag("Voice"))
            {
                source.volume = voiceVolume * masterVolume;
            }
            else
            {
                source.volume = sfxVolume * masterVolume;
            }
        }
    }
}
