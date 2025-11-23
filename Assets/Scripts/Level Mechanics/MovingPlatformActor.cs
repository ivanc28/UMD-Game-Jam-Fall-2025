using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

/// <summary>
/// Moving Platform that is triggered by a lightreceiver
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformActor : LightActor
{
    public Vector3 finalPosition;
    public float speed;
    public Light2D activatedLight;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Rigidbody2D rb;

    public void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        targetPosition = initialPosition;

    }

    public void FixedUpdate() 
    {
        if (rb.velocity.magnitude > 0)
        {
            // stop rb from overshooting
            if (PlatformOvershot()) 
            {
                SetPosToFinal();
            }
        }
    }

    public override void Activate()
    {
        activatedLight.enabled = true;
        if (PlatformOvershot())
        {
            SetPosToFinal();
            return;
        }
        Vector3 vel = (finalPosition - transform.position).normalized * speed;
        targetPosition = finalPosition;
        rb.velocity = vel;
    }

    public override void Deactivate()
    {
        activatedLight.enabled = false;
        Vector3 vel = (initialPosition - targetPosition).normalized * speed;
        targetPosition = initialPosition;
        rb.velocity = vel;
    }
    private bool PlatformOvershot()
    {
        // if the direction to initial is same as to final, the object must have overshot
        return Vector3.Dot(targetPosition - transform.position, rb.velocity) < 0;
    }
    private void SetPosToFinal()
    {
        transform.position = targetPosition;
        rb.velocity = Vector2.zero;
    }
}
