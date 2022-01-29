using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "SO/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField] private List<SoundData> soundData;

    public SoundData GetSound(SoundEvent audioEvent)
    {
        return soundData.FirstOrDefault(x => x.soundEvent == audioEvent);
    }

}

public enum SoundEvent
{
    StartClicked,
    ButtonClicked
}

[Serializable]
public class SoundData
{
    public SoundEvent soundEvent;
    public AudioClip audioClip;
    [Range(0.0f, 1.0f)] public float volume = 1.0f;
}