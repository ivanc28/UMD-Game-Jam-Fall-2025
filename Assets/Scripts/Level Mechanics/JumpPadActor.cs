using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class JumpPadActor : LightActor
{
    public float jumpStrength;
    bool isActive;
    private void Start()
    {
        isActive = false;
    }
    public override void Activate()
    {
        isActive = true;
    }

    public override void Deactivate()
    {
        isActive = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActive)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Vector3 plrVel = player.GetComponent<Rigidbody2D>().velocity;
                plrVel.y = jumpStrength;
                player.GetComponent<Rigidbody2D>().velocity = plrVel;
            }
        }
    }
}
