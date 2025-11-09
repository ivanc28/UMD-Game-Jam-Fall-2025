using System;
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
    private Vector2 direction;

    /// <summary>
    /// speed of projectile in meters per second
    /// </summary>
    public float speed;

    /// <summary>
    /// duration (in ticks) of projectile.
    /// </summary>
    public float duration;

    /// <summary>
    /// flag for halting motion
    /// </summary>
    private bool expiring;

    private Rigidbody2D rb;

    private void Start()
    {
        expiring = false;
        //lol quaternions
        direction = Vector2.right * (float)Math.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z); 
        transform.rotation = Quaternion.identity; // realigns gameobject to have 0 rotation (prolly not needed
        //Debug.Log("Direction: " + direction);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

    }


    void Update()
    {   
        if(!expiring)
        {
            if (duration <= 0)
            {
                Expire();
            }
            duration -= Time.deltaTime;
        }
    }

    // Two cases, receivers, and non-receivers
    // colliding with receivers should call the receiver's respective intake method (TBI)
    // non receivers should cause the projectile to halt and expire.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        LightReceiver receiver = collision.gameObject.GetComponent<LightReceiver>();
        if (receiver != null)
        {
            receiver.Intake(this);
            Debug.Log("Hello?");
        }
        Debug.Log("Nope");
        Expire();
    }

    /// <summary>
    /// Called when the projectile has exceeded its duration. Currently destroys the object.
    /// </summary>
    private void Expire()
    {
        expiring = true;
        Destroy(gameObject);
    }

    
}
