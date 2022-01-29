using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public Action<bool> onGamePaused;
    [SerializeField] GameObject pauseScreen;

    public void PauseGame(bool toggle)
    {
        onGamePaused?.Invoke(toggle);
        pauseScreen.SetActive(toggle);
    }
}
