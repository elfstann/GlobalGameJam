using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPopup : Popup
{
    [SerializeField] private CustomButton exitButton;
    [SerializeField] private ClickHolder fader;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnCloseClicked);
        fader.OnClickEvent += OnCloseClicked;
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(OnCloseClicked);
        fader.OnClickEvent -= OnCloseClicked;
    }

    private void OnCloseClicked()
    {
        Close();
    }
}
