using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    [SerializeField] Transform destination;
    public bool isActive {get;set;} = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if (collision.gameObject.tag != "Player") return;
        PlayerController.Instance.transform.position = destination.position;
    }
}
