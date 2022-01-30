using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private MainMenu menu;
    [SerializeField] private AudioClip introClip;
    private IntroItem[] items;

    private void Awake()
    {
        items = GetComponentsInChildren<IntroItem>();

        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
    }

    //private IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    var audioManager = AudioPlayer.Instance;
    //    audioManager.PlayIntro(introClip);
    //    items[0].Play();

    //    for (int i = 1; i < items.Length; i++)
    //    {
    //        var item = items[i];

    //        yield return new WaitUntil(() => item.StartTime <= audioManager.CurrentSoundTime);
    //        item.Play();
    //    }

    //    yield return new WaitForSeconds(10.0f);
    //    menu.SceneLoading.allowSceneActivation = true;
    //}
}
