using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoPopUp : Popup
{
    private void OnCloseClicked()
    {
        SceneLoader.LoadMenu();
    }
}
