using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecleTrap : BaseTrap
{
    [SerializeField] Trigger trapTrigger;
    [SerializeField] Rigidbody2D icicleRigidbody;
    [SerializeField] SpriteRenderer icecleSprite;
    [SerializeField] Sprite icecleOnFallSprite;

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
        if (AudioPlayer.Instance != null) AudioPlayer.Instance.PlaySound(SoundEvent.TrapIce);
        icecleSprite.sprite = icecleOnFallSprite;
        icicleRigidbody.gravityScale = 1f;
    }
}
