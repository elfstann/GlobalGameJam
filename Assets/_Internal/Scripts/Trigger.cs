using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] Collider2D triggerCollider;
    public Action OnTrapTrigger;

    private void Awake()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            OnTrapTrigger?.Invoke();
            DestroyTrigger();
        }
    }
    private void DestroyTrigger()
    {
        OnTrapTrigger = null;
        Destroy(this);
    }
}
