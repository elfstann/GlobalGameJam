using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Data/AudioDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField] private List<SoundData<SoundEvent>> soundData;
    [SerializeField] private List<SoundData<MusicEvent>> musicData;

    public SoundData<SoundEvent> GetSound(SoundEvent soundEvent)
    {
        return soundData.FirstOrDefault(x => x.audioEvent == soundEvent);
    }

    public SoundData<MusicEvent> GetMusic(MusicEvent musicEvent)
    {
        return musicData.FirstOrDefault(x => x.audioEvent == musicEvent);
    }
}

public enum SoundEvent
{
    StartClicked,
    ButtonClicked,
    DeathRabbit,
    DeathBear,
    Jump,
    Walk,
    TrapIce,
    TrapRock
}

public enum MusicEvent
{
    MainMenuTheme,
    Level01,
    Level02,
    Level03,
    Level04,
    Level05,
    Level06
}

[Serializable]
public class SoundData<T>
{
    public T audioEvent;
    public AudioClip audioClip;
    [Range(0.0f, 1.0f)] public float volume = 1.0f;
}