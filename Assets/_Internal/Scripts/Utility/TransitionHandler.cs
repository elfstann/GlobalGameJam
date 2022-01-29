using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 1.5f;
    [SerializeField] private bool sin = true;

    public void DoTransition(SceneIndex sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(SceneIndex sceneName, Action onSceneLoaded = null)
    {
        canvasGroup.blocksRaycasts = true;
        yield return Fade(0.0f, 1.0f, fadeInDuration);
        var sceneLoading = SceneManager.LoadSceneAsync((int)sceneName, LoadSceneMode.Single);
        sceneLoading.allowSceneActivation = false;

        sceneLoading.allowSceneActivation = true;
        while (!sceneLoading.isDone)
        {
            yield return null;
        }

        onSceneLoaded?.Invoke();
        yield return Fade(1.0f, 0.0f, fadeOutDuration);
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0.0f;
        canvasGroup.alpha = from;

        while (time < duration)
        {
            var t = time / duration;
            t = sin ? Mathf.Sin(t * Mathf.PI * 0.5f) : 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }


}
