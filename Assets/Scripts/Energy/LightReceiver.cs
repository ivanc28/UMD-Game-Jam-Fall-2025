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
        Init();
    }

    public virtual void Init() { }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Received?");
    //    LightProjectile lightProjectile = collision.gameObject.GetComponent<LightProjectile>();
    //    if (lightProjectile != null)
    //    {
    //        Intake(lightProjectile);
    //    }
    //}

    public void Intake(LightSender lightSender)
    {
        if (curLight < lightTreshold)
        {
            curLight += lightSender.GetLightValue();
            if (curLight >= lightTreshold)
            {
                Activate();
            }
        }
    }

    public abstract void Activate();
}
