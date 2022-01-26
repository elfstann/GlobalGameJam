using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] public int itemId;
    [SerializeField] public string itemName;
    [SerializeField] GameObject vfxOnPick;
    [SerializeField] bool destroyOnPick = true;
    public void Pick()
    {
        if (vfxOnPick != null) Instantiate(vfxOnPick, transform.position, Quaternion.identity);
        if (destroyOnPick) Destroy(this.gameObject);
    }    
}
