using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseTrap : MonoBehaviour
{
    [SerializeField] int damagePerHit;
    [SerializeField] GameObject vfxOnHit;
    [SerializeField] Collider2D trapCollider;

    private void Awake()
    {
        if (trapCollider == null)
            trapCollider = GetComponent<Collider2D>();
    }

    public virtual void TriggerTrap()
    {
        Debug.Log("No Trap Trigger");
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.CurrentHealthController.CurrentHealth -= damagePerHit;
        }

        if (vfxOnHit != null) Instantiate(vfxOnHit, collision.GetContact(0).point, Quaternion.identity);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
