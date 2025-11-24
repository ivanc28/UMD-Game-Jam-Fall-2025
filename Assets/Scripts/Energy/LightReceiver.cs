using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Script for all objects that will collide with lightSenders.
/// Once they receive a lightSender with lightValue at or above the lightThreshold, they will activate the LightActor field.
/// If you want a gameobject to be both a LightReceiver and LightActor, put both scripts as components and set this script's actor 
/// as itself.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LightReceiver : MonoBehaviour
{
    /// <summary>
    /// lightvalue to activate actor
    /// </summary>
    [Tooltip("lightvalue to activate actor")]
    public int lightTreshold = 10;

    /// <summary>
    /// Duration in seconds
    /// </summary>
    [Tooltip("Duration in seconds")]
    public float activateDuration = 5;
    [Tooltip("LightActor object that will activate when this receiver receivse enough light")]
    public LightActor actor;
    private float duration;
    public bool permanent;
    private bool activated;

    public Light2D activatedLight;

    public void Start()
    {
        Init();
    }

    public virtual void Init() { }

    public void Update()
    {
        if (!permanent && activated)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                activated = false;
                actor.Deactivate();
            }
        }
        if(activatedLight != null)
        {
            if (activated)
            {
                activatedLight.enabled = true;
            }
            else
            {
                activatedLight.enabled = false;
            }
        }
    
    }

    // TODO: Rework so that it doesn't accumulate light, but instead activates once enough light is immediately sent?
    // Could change to this
    public void Intake(LightSender lightSender)
    {
        if (lightSender.GetLightValue() >= lightTreshold)
        {
            actor.Activate();
            duration = activateDuration;
            activated = true;

        }
    }
    public void Intake(int lightValue)
    {
        if (lightValue >= lightTreshold)
        {
            actor.Activate();
            duration = activateDuration;
            activated = true;
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
    //public abstract void Activate();
}
