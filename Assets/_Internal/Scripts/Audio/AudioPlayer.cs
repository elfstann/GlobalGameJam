using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private SoundDatabase audioDatabase;

    public static AudioPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayMusic()
    {
        musicPlayer.Play();
    }

    public void PlaySound(SoundEvent soundEvent)
    {
        var data = audioDatabase.GetSound(soundEvent);
        if (data == null)
        {
            Debug.LogWarning($"No data for '{soundEvent}' event");
            return;
        }

        soundPlayer.PlayOneShot(data.audioClip, data.volume);
    }

    private void OnMusicMute(bool value)
    {
        musicPlayer.mute = value;
    }

    private void OnSoundsMute(bool value)
    {
        soundPlayer.mute = value;
    }
}

