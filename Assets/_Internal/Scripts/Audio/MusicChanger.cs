using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private List<MusicEvent> musicCollection;
    [SerializeField] private bool OneAndLooped;

    private Stack<MusicEvent> randomStack;

    private IEnumerator Start()
    {
        if (AudioPlayer.Instance == null) yield break;

        if (OneAndLooped)
        {
            PlayRandom(true);
            yield break;
        }

        PlayRandom(false);

        while (true)
        {
            yield return new WaitWhile(() => AudioPlayer.Instance.IsMusicPlaying);
            Debug.Log($"Not played!");
            PlayRandom(false);
            yield return new WaitWhile(() => !AudioPlayer.Instance.IsMusicPlaying);
            Debug.Log($"Next played!");
        }
    }

    private void PlayRandom(bool isLoop)
    {
        if (randomStack == null || randomStack.Count == 0)
        {
            randomStack = musicCollection.ToRandomStack();
        }

        var music = randomStack.Pop();

        AudioPlayer.Instance.PlayMusic(music, isLoop);
    }
}
