using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseTrap : MonoBehaviour
{
    [SerializeField] int damagePerHit;
    [SerializeField] GameObject vfxOnHit;
    [SerializeField] Collider2D trapCollider;
    [SerializeField] bool destroyOnItemCollision = true;
    [SerializeField] bool destroyOnPlayerCollision = true;

    Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        if (trapCollider == null)
            trapCollider = GetComponent<Collider2D>();

        PauseManager.Instance.OnGamePaused += PauseTrapPhysics;
    }

    private void PauseTrapPhysics(bool state)
    {
        Rigidbody2D rigidBody;
        if(TryGetComponent(out rigidBody)){ if (state) { velocity = rigidBody.velocity; rigidBody.Sleep(); } else { rigidBody.WakeUp(); rigidBody.velocity = velocity; } }
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
            PlayerController.Instance.ShakeCamera(2.5f, .5f);
            if (destroyOnPlayerCollision) Destroy(gameObject);
            if (vfxOnHit != null) Instantiate(vfxOnHit, collision.GetContact(0).point, Quaternion.identity);
        }

        if (destroyOnItemCollision && gameObject != null)
        {
            Destroy(gameObject);
            if (vfxOnHit != null) Instantiate(vfxOnHit, collision.GetContact(0).point, Quaternion.identity);
        } 
    }

    private void OnDestroy()
    {
        PauseManager.Instance.OnGamePaused -= PauseTrapPhysics;
    }
}
