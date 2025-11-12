using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Placeholder object to test lightreceiver code
/// If we want to use this seriously, gotta add gizmos and such to better design target position
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformReceiver : LightActor
{
    public Vector2 finalDisplacement;
    public float moveSpeed;
    
    Vector2 initialPosition;
    Vector2 finalPosition;
    Vector2 direction;
    Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        finalPosition = finalDisplacement + initialPosition;
        rb.gravityScale = 0;
        rb.isKinematic = true;
    }
    public override void Activate()
    {
        direction = finalDisplacement.normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void FixedUpdate()
    {
        if (rb.velocity.magnitude != 0)
        {
            if(Vector2.Dot((finalPosition - rb.position), direction) < 0)
            {
                rb.velocity = Vector2.zero;
                rb.position = finalPosition;
            }
        }
    }

    public override void Deactivate()
    {
        
    }

}
