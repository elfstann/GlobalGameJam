using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] public int itemId;
    [SerializeField] GameObject vfxOnPick;
    [SerializeField] bool destroyOnPick = true;
    [SerializeField] UnityEvent onPick = new UnityEvent();
    public void Pick()
    {
        if (vfxOnPick != null) Instantiate(vfxOnPick, transform.position, Quaternion.identity);
        onPick.Invoke();
        if (destroyOnPick) Destroy(this.gameObject);
    }    
}
