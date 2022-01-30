using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private SoundDatabase audioDatabase;
    [SerializeField] private float fadeDuration = 0.4f;

    public static AudioPlayer Instance { get; private set; }

    public bool IsMusicPlaying => musicPlayer.isPlaying;

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

    public void PlayMusic(MusicEvent musicEvent, bool isLoop = false)
    {
        var data = audioDatabase.GetMusic(musicEvent);
        if (data == null)
        {
            Debug.LogWarning($"No data for '{musicEvent}' event");
            return;
        }

        musicPlayer.DOComplete(true);
        musicPlayer.DOFade(0.0f, fadeDuration).OnComplete(() =>
        {
            musicPlayer.clip = data.audioClip;
            musicPlayer.Play();
            musicPlayer.loop = isLoop;
            musicPlayer.DOFade(data.volume, fadeDuration);
        });
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

    public void StopMusic()
    {
        musicPlayer
            .DOFade(0.0f, fadeDuration)
            .OnComplete(() =>
            {
                musicPlayer.Stop();
            });
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

