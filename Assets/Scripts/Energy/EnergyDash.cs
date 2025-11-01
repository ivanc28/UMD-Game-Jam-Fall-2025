using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnergyDash : EnergyMove
{
    private Rigidbody2D playerRb;
    public Vector2 direction;
    public float power;

    public override void Activate()
    {
        playerRb.AddForce(direction * power);
    }

    protected override void MoveUpdate()
    {
        if (playerRb.velocity.magnitude > 0)
        {
            direction = (Vector2.Dot(playerRb.velocity, Vector2.right) * Vector2.right).normalized;
        }
    }

    protected override void GetComponents()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
}
