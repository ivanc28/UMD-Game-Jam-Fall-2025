using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

/// <summary>
/// Script for light projectiles. Requires a collider component. 
/// <br></br>
/// Upon start, the projectile will constantly travel in it's direction vector. This projectile is not affected by gravity.
/// </summary>
public class LightProjectile : MonoBehaviour
{
    /// <summary>
    /// light value of projectile. Used for accumulating light in light receivers.
    /// </summary>
    public int lightValue;

    /// <summary>
    /// direction of projectile
    /// </summary>
    public Vector2 direction;

    /// <summary>
    /// speed of projectile in meters per tick
    /// </summary>
    public float speed;

    /// <summary>
    /// duration (in seconds) of projectile.
    /// </summary>
    public float duration;

    /// <summary>
    /// flag for halting motino
    /// </summary>
    private bool expiring;


    private void Start()
    {
        expiring = false;
    }


    void FixedUpdate()
    {   
        if(!expiring)
        {
            if (duration <= 0)
            {
                expiring = true;
                Expire();
            }
            else
            {
                transform.Translate(direction * speed);
            }
            duration -= Time.deltaTime;
        }
    }

    // Two cases, receivers, and non-receivers
    // colliding with receivers should call the receiver's respective intake method (TBI)
    // non receivers should cause the projectile to halt and expire.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        expiring = true;
    }

    /// <summary>
    /// Called when the projectile has exceeded its duration. Currently destroys the object.
    /// </summary>
    private void Expire()
    {
        Destroy(gameObject);
    }
}
