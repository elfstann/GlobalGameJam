using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : Popup
{
    [SerializeField] private CustomButton resumeButton;
    [SerializeField] private CustomButton exitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        exitButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        exitButton.onClick.RemoveListener(OnCloseClicked);
    }

    private void OnResumeClicked()
    {
        PauseManager.Instance.PauseGame(false);
    }

    private void OnCloseClicked()
    {
        SceneLoader.LoadMenu();
    }
}
