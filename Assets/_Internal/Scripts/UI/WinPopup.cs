using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : Popup
{
    [SerializeField] private CustomButton exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(OnCloseClicked);
    }

    private void OnCloseClicked()
    {
        SceneLoader.LoadMenu();
    }
}
