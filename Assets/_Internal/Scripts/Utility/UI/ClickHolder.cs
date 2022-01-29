using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHolder : MonoBehaviour, IPointerDownHandler
{
    public event Action OnClickEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickEvent?.Invoke();
    }
}