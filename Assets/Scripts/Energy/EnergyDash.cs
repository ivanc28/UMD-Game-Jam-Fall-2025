using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnergyDash : EnergyMove
{
    [Tooltip("Speed of dash")]
    public float speed;
    [Tooltip("Duration (in ticks) of dash")]
    public int duration;
    public GameObject projectilePrefab;

    private Vector2 direction;
    private Player player;
    private Rigidbody2D playerRb;
    private int remainingDuration;
    private bool dashing;


    //TODO: grab direction as a vector to allow omnidirectional dashing. Gotta record some more input
    public override void Activate()
    {
        remainingDuration = duration;
        dashing = true;
        player.canDash = false;
        
        player.DisableMovementControl();
        player.SetGravityScale(0);

        Vector2 direction = Vector2.right * (player.dir);
        Quaternion rotation = Quaternion.Euler(0, 0, 90 + (player.dir * 90));

        playerRb.velocity = direction * speed;

        Debug.Log("Calculated rotation is:" + rotation.eulerAngles.z);
        Debug.Log("Shooting Projectile in direction:" + Vector2.right * (float)Math.Cos(Mathf.Deg2Rad *rotation.eulerAngles.z));
        GameObject proj = Instantiate(projectilePrefab, transform.position, rotation);

    }

    protected override void MoveUpdate()
    {

    }

    protected override void MoveFixedUpdate()
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
                player.SetGravityScale(player.defaultGravity);
                dashing = false; // flag to stop continuously calling this
            }
        }
    }

    protected override void Initialize()
    {
        playerRb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        direction = Vector2.right;
        remainingDuration = 0;
        dashing = false;
    }
}
