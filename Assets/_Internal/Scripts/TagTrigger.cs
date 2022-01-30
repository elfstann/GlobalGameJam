using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TagTrigger : MonoBehaviour
{
    [SerializeField] string otherTag;
    [SerializeField] UnityEvent onTriggerEnter = new UnityEvent();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != otherTag) return;
        onTriggerEnter.Invoke();
    }
}
