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
        
        player.DisableMovementControl();

        Vector2 direction = Vector2.right * (player.dir);

        GameObject proj = Instantiate(projectilePrefab);
        LightProjectile projComp = projectilePrefab.GetComponent<LightProjectile>();
        playerRb.velocity = direction * speed;
        projComp.direction = direction * -1;
        proj.transform.position = transform.position;


        Debug.Log("Dashing in direction:" + playerRb.velocity.normalized);
        Debug.Log("Shooting Projectile in direction:" + projComp.direction);

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
