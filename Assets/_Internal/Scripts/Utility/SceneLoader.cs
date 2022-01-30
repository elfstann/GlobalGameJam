using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    Menu = 0,
    Game = 1
}

public static class SceneLoader
{
    private static TransitionHandler transition;

    private static void CheckBlack()
    {
        if (transition == null)
        {
            var prefab = Resources.Load<TransitionHandler>("TransitionCanvas");
            transition = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(transition);
        }
    }

    private static void LoadWithTransition(SceneIndex sceneName)
    {
        CheckBlack();

        transition.DoTransition(sceneName);
    }

    public static void LoadGame()
    {
        LoadWithTransition(SceneIndex.Game);
    }

    public static void LoadMenu()
    {
        LoadWithTransition(SceneIndex.Menu);
    }

}
