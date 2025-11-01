using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnergyDash : EnergyMove
{
    [Tooltip("Speed of dash")]
    public float speed;
    [Tooltip("Duration (in ticks) of dash")]
    public int duration;

    private float direction;
    private Player player;
    private Rigidbody2D playerRb;
    private int remainingDuration;
    private bool dashing;

    public override void Activate()
    {
        playerRb.velocity = new Vector2(direction * speed, 0);
        remainingDuration = duration;
        dashing = true;
        player.DisableMovementControl();
    }

    protected override void MoveUpdate()
    {
        if (dashing)
        {
            if (remainingDuration > 0)
            {
                remainingDuration--;
            }
            else
            {
                player.EnableMovementControl();
                dashing = false; // flag to stop continuously calling this
            }
        }
        else if (Math.Abs(playerRb.velocity.x) > 0)
        {
            direction = Mathf.Clamp(playerRb.velocity.x, -1, 1);
        }
        
    }

    protected override void Initialize()
    {
        playerRb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        direction = 1;
        remainingDuration = 0;
        dashing = false;
    }
}
