using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecleTrap : BaseTrap
{
    [SerializeField] Trigger trapTrigger;
    [SerializeField] Rigidbody2D icicleRigidbody;

    private void Start()
    {
        if (icicleRigidbody == null)
            icicleRigidbody = GetComponent<Rigidbody2D>();

        trapTrigger.OnTrapTrigger += TriggerTrap;
    }
    public override void TriggerTrap()
    {
        base.TriggerTrap();
        Debug.Log("Triggered");
        icicleRigidbody.gravityScale = 1f;
    }
}
