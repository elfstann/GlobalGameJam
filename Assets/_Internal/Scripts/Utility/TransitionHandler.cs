using DG.Tweening;
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
    private readonly Ease ease = Ease.OutSine;

    public void DoTransition(SceneIndex sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(SceneIndex sceneName, Action onSceneLoaded = null)
    {
        // Fade in
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0.0f;
        var fadeInTween = canvasGroup.DOFade(1.0f, fadeInDuration).SetEase(ease);
        yield return fadeInTween.WaitForCompletion();

        // Load scene
        var sceneLoading = SceneManager.LoadSceneAsync((int)sceneName, LoadSceneMode.Single);
        sceneLoading.allowSceneActivation = false;
        sceneLoading.allowSceneActivation = true;
        while (!sceneLoading.isDone)
        {
            yield return null;
        }

        onSceneLoaded?.Invoke();

        // Fade out
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1.0f;
        var fadeOutTween = canvasGroup.DOFade(0.0f, fadeOutDuration).SetEase(ease);
        yield return fadeOutTween.WaitForCompletion();

    }

}
