using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class LightReceiver : MonoBehaviour
{
    public int lightTreshold;
    private int curLight;

    public void Start()
    {
        curLight = 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        LightProjectile lightProjectile = collision.gameObject.GetComponent<LightProjectile>();
        if (lightProjectile != null)
        {
            Intake(lightProjectile);
        }
    }

    public void Intake(LightProjectile lightProjectile)
    {
        if (curLight < lightTreshold)
        {
            curLight += lightProjectile.lightValue;
            if (curLight >= lightTreshold)
            {
                Activate();
            }
        }
    }

    public abstract void Activate();
}
