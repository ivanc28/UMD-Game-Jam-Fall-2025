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

    // TODO: Rework so that it doesn't accumulate light, but instead activates once enough light is immediately sent?
    // Could change to this
    public void Intake(LightSender lightSender)
    {
        if (lightSender.GetLightValue() >= lightTreshold)
        {
            Activate();
        }
    }
    public void Intake(int lightValue)
    {
        if (lightValue >= lightTreshold)
        {
            Activate();
        }
    }
    //public void Intake(LightSender lightSender)
    //{
    //    if (curLight < lightTreshold)
    //    {
    //        curLight += lightSender.GetLightValue();
    //        if (curLight >= lightTreshold)
    //        {
    //            Activate();
    //        }
    //    }
    //}
    //public void Intake(int lightValue)
    //{
    //    if (curLight < lightTreshold)
    //    {
    //        curLight += lightValue;
    //        if (curLight >= lightTreshold)
    //        {
    //            Activate();
    //        }
    //    }
    //}
    /// <summary>
    /// Optional to implement a Deactivate method, like once a timer expires
    /// </summary>
    public virtual void Deactivate() { }
    public abstract void Activate();
}
