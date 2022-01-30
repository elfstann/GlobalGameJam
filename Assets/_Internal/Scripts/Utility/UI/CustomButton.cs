using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    protected override void Awake()
    {
        base.Awake();

        onClick.AddListener(PlaySound);
    }

    protected override void OnDestroy()
    {
        onClick.RemoveListener(PlaySound);

        base.OnDestroy();
    }

    private void PlaySound()
    {
        if (AudioPlayer.Instance == null) return;

        AudioPlayer.Instance.PlaySound(SoundEvent.ButtonClicked);
    }
}
