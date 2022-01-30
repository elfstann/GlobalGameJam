using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ItemPicker : MonoBehaviour
{
    [SerializeField] List<int> collectedItems = new List<int>();
    PlayerInputScheme inputScheme;

    ItemChecker lastChecker;

    private void Start()
    {
        inputScheme = new PlayerInputScheme();
        inputScheme.Player.Fire.Enable();
        inputScheme.Player.Fire.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext obj) 
    {
        if (lastChecker == null) return;
        if (PlayerController.Instance.currentPlayerState == PlayerState.Bear)
            lastChecker.CheckItems();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickableItem item = collision.gameObject.GetComponent<PickableItem>();
        if (item != null) 
        {
            collectedItems.Add(item.itemId);
            item.Pick();
        }

        ItemChecker checker = collision.gameObject.GetComponent<ItemChecker>();
        if (checker != null)
        {
            if (checker.requireInteraction) 
            {
                lastChecker = checker;
            }
            else
            {
                checker.CheckItems();
            }
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemChecker checker = collision.gameObject.GetComponent<ItemChecker>();
        if (checker == null) return; 
        lastChecker = null;
    }



    public bool HasItem(int id) 
    {
        return collectedItems.Contains(id);
    }
    public bool RemoveItem(int id)
    {
        return collectedItems.Remove(id);
    }
    public void RemoveItems(IEnumerable<int> itemsId) 
    {
        collectedItems.RemoveAll(x => itemsId.Contains(x));
    }
}
