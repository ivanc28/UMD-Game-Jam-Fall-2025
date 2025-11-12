using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]

/// <summary>
/// Script for light projectiles. Requires a collider component. 
/// <br></br>
/// Upon start, the projectile will constantly travel in it's direction vector. This projectile is not affected by gravity.
/// </summary>
public class LightProjectile : LightSender
{   

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
    public float maxDuration;
    private float duration;

    /// <summary>
    /// flag for halting motion
    /// </summary>
    private bool expiring;

    private Rigidbody2D rb;

    public override void Init()
    {
        base.Init();
        duration = maxDuration;

        expiring = false;
        //lol quaternions
        direction = Vector2.right * (float)Math.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.identity; // realigns gameobject to have 0 rotation (prolly not needed
        //Debug.Log("Direction: " + direction);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }
    public override void MakeUpdate()
    {
        if (!expiring)
        {
            if (duration <= 0)
            {
                Expire();
            }
            duration -= Time.deltaTime;
            FadeLight();
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
            //Debug.Log("Hello?");
        }
        //Debug.Log("Nope");
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

    /// <summary>
    /// Fades the light's intensity and size based on lifespan (duration)
    /// </summary>
    private void FadeLight()
    {
        float t = Mathf.Sqrt(duration / maxDuration);
        SetLightStrengthLerp(t);
    }

    
}
