using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public event Action<bool> OnGamePaused;
    [SerializeField] private PopupHandler popupHandler;

    public void PauseGame(bool toggle)
    {
        if (toggle)
        {
            popupHandler.Open(PopupName.Pause, _ => OnGamePaused?.Invoke(toggle));
        }
        else
        {
            popupHandler.CloseCurrent();
            OnGamePaused?.Invoke(toggle);
        }
    }

    public void WinGame()
    {
        popupHandler.Open(PopupName.Win, _ => OnGamePaused?.Invoke(true));
    }

    public void EndGame()
    {
        popupHandler.Open(PopupName.Death, _ => OnGamePaused?.Invoke(true));
    }
}
