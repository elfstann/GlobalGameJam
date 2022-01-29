using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private PopupHandler popupHandler;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
        creditsButton.onClick.RemoveListener(OnCreditsClicked);
        exitButton.onClick.RemoveListener(OnExitClicked);
    }

    public void OnPlayClicked()
    {
        AudioPlayer.Instance.PlaySound(SoundEvent.StartClicked);
        SceneLoader.LoadGame();
    }

    public void OnCreditsClicked()
    {
        popupHandler.Open(PopupName.Credits);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
