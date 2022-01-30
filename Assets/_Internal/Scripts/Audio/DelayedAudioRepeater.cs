using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudioRepeater : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minDelay = 2.0f;
    [SerializeField] private float maxDelay = 5.0f;

    private IEnumerator Start()
    {
        var delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        Play();

        while (true)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);

            delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            Play();
        }
    }

    private void Play()
    {
        audioSource.Play();
    }

}
