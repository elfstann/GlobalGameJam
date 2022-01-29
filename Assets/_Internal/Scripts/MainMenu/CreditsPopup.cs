using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPopup : Popup
{
    [SerializeField] private CustomButton exitButton;
    [SerializeField] private ClickHolder fader;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnCloseClick);
        fader.OnClickEvent += OnCloseClick;
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(OnCloseClick);
        fader.OnClickEvent -= OnCloseClick;
    }

    private void OnCloseClick()
    {
        Close();
    }
}
