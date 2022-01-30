using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum PopupName
{
    Credits = 1,
    Pause = 2
}

public class PopupHandler : MonoBehaviour
{
    [SerializeField] private List<Popup> popups;

    private readonly float openDuration = 0.5f;
    private readonly Ease openEase = Ease.OutSine;
    private readonly Ease closeEase = Ease.OutSine;
    private readonly float closeDuration = 0.2f;
    private Sequence openSequence;
    private Sequence closeSequence;

    private Popup currentlyOpen;

    private void Awake()
    {
        foreach (var popup in popups)
        {
            popup.Close();
            popup.OnClose += OnClose;
        }
    }

    private void OnDestroy()
    {
        foreach (var popup in popups)
        {
            popup.OnClose -= OnClose;
        }
    }

    private void OnClose(Popup popup)
    {
        if (closeSequence != null) closeSequence.Kill();

        popup.CanvasGroup.alpha = 1.0f;
        closeSequence = DOTween.Sequence()
            .Append(popup.CanvasGroup.DOFade(0.0f, closeDuration))
            .OnComplete(() =>
            {
                popup.gameObject.SetActive(false);
            })
            .SetEase(closeEase)
            .Play();

        currentlyOpen = null;
    }

    public void CloseCurrent()
    {
        if (currentlyOpen != null && currentlyOpen.gameObject.activeInHierarchy == true)
            currentlyOpen.Close();
    }

    public void Open(PopupName name, Action<Popup> init = null)
    {
        // Check for currently open
        if (currentlyOpen != null && currentlyOpen.gameObject.activeInHierarchy == true)
        {
            if (currentlyOpen.Name == name)
            {
                Debug.LogWarning($"Popup '{name}' is already open");
                return;
            }
            currentlyOpen.Close();
        }

        if (closeSequence != null) closeSequence.Kill(true); // Instantly complete closing

        // Find one to open
        var popup = popups.FirstOrDefault(x => x.Name == name);
        if (popup == null)
        {
            Debug.LogWarning($"Popup '{name}' not found");
            return;
        }

        // Open
        popup.CanvasGroup.alpha = 0.0f;
        init?.Invoke(popup);
        popup.Open();

        if (openSequence != null) openSequence.Kill();

        openSequence = DOTween.Sequence()
            .Append(popup.CanvasGroup.DOFade(1.0f, openDuration))
            .SetEase(openEase)
            .Play();

        currentlyOpen = popup;
    }
}

