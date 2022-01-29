using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public PopupName Name;
    [SerializeField] private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;

    public event Action<Popup> OnClose;

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Init<T>(T data)
    {

    }

    public void Close()
    {
        OnClose?.Invoke(this);
    }



}
