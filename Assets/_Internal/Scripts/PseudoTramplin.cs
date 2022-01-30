using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PseudoTramplin : MonoBehaviour
{
    [SerializeField] private int force = 20;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private Animator animator;

    private bool isJumpAvailable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            if (!isJumpAvailable) return;

            Rigidbody2D player = collision.gameObject.transform.parent.GetComponent<Rigidbody2D>();
            if (player == null) return;

            StartCoroutine(JumpRoutine(player));
        }
    }

    private IEnumerator JumpRoutine(Rigidbody2D player)
    {
        isJumpAvailable = false;
        player.velocity = new Vector2(player.velocity.x, force);
        if (AudioPlayer.Instance != null) AudioPlayer.Instance.PlaySound(SoundEvent.Jump);
        animator.SetTrigger("Jump");

        yield return new WaitForSeconds(delay);
        isJumpAvailable = true;
    }
}
