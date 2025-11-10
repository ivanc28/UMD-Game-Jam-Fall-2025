using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRest : EnergyMove
{
    RestLampReceiver receiver;
    public override void Activate()
    {        
        if (receiver != null)
        {
            // If the rest lamp hasn't been activated yet, we activate it
            if (!receiver.lightsOn)
            {
                receiver.Intake(energyCost);
            }
        }
    }

    protected override bool CanUse()
    {
        return base.CanUse() && receiver != null;
    }
    protected override bool EscapeEnergyDeplete()
    {
        // For when rest lamp is already active but we are touching the collider
        if (receiver != null)
        {
            return receiver.lightsOn;
        }
        return false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<RestLampReceiver>() != null)
        {
            receiver = collision.gameObject.GetComponent<RestLampReceiver>();
        }        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<RestLampReceiver>() != null)
        {
            receiver = null;
        }
    }
}
