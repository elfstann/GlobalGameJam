using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PseudoTramplin : MonoBehaviour
{
    [SerializeField] int force = 20;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Rigidbody2D player = collision.gameObject.transform.parent.GetComponent<Rigidbody2D>();
            player.velocity = new Vector2(player.velocity.x, force);
        }
    }
}
