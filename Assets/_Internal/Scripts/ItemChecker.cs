using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    [SerializeField] List<int> requiredItems;
    [SerializeField] bool removeItems;

    [Tooltip("When set to true, require player to press action btn to preform the check")]
    [SerializeField] public bool requireInteraction = false;

    [SerializeField] bool destroyOnSuccess;
    [SerializeField] UnityEvent onCheckSuccess = new UnityEvent();
    [SerializeField] UnityEvent onCheckFail = new UnityEvent();
    

    public void CheckItems() 
    {
        var picker = PlayerController.Instance.itemPicker;
        foreach (int iid in requiredItems)
        {
            if (!picker.HasItem(iid))
            {
                onCheckFail.Invoke();
                return;
            }
        }
        if (removeItems)
            picker.RemoveItems(requiredItems);
        if (destroyOnSuccess)
            Destroy(this);
        onCheckSuccess.Invoke();
        return;
    }
}
