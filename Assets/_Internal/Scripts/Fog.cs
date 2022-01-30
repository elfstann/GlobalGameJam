using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform firefly;

    [SerializeField]
    float agrorange;

    [SerializeField]
    float movespeed;

    Rigidbody2D rb2d;
    bool SeeLeft;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        PositionPlayer();


        if (SeePlayer(agrorange))
        {
            GoBack();
        }
        else
        {
            if (distToPlayer < agrorange)
            {
                ChasePlayer();
            }
            else
            {
                StopChasing();
            }
        }

    }

    void ChasePlayer()
    {
        if (SeeLeft)
        {
            rb2d.velocity = new Vector2(movespeed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(-movespeed, 0);
        }
    }
    void PositionPlayer()
    {
        if (transform.position.x < player.position.x)
        {
            SeeLeft = false;
        }
        else
        {
            SeeLeft = true;
        }
    }

    void StopChasing()
    {
        rb2d.velocity = new Vector2(0, 0);
    }
    void GoBack()
    {
        if (SeeLeft)
        {
            rb2d.velocity = new Vector2(-movespeed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(movespeed, 0);
        }
    }

    bool SeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (SeeLeft)
        {
            castDist = -distance;
        }

        Vector2 endPose = firefly.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Raycast(firefly.position, endPose, 1 << LayerMask.NameToLayer("Action"));
        
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Fog"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(firefly.position, hit.point, Color.yellow);
        }
        else
        {
            Debug.DrawLine(firefly.position, endPose, Color.blue);
        }
        return val;
   
    }
}
