using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnergyMove : MonoBehaviour
{
    [Tooltip("Keycode that activates move")]
    /// <summary>
    /// input key that triggers the move
    /// </summary>
    public KeyCode key;

    [Tooltip("Energy consumed upon triggering move")]
    public int energyCost;

    [Tooltip("Cooldown in quantities of ticks")]
    /// <summary>
    /// How many ticks before move can be used again.
    /// </summary>
    public int cooldown;

    // TODO: Should we use cooldowns or a CanUse flag for when after ur grounded?
    protected int cooldownRemaining;
    
    /// <summary>
    /// field for if we want moves to have a one time use until a condition is met (regrounded or such)
    /// </summary>
    protected bool consumed;
    
    // Start is called before the first frame update
    public void Start()
    {
        consumed = false;
        cooldownRemaining = 0;
        Initialize();
    }

    /// <summary>
    /// Called at start to grab component references
    /// </summary>
    protected virtual void Initialize() { }


    // Update is called once per frame
    public void Update()
    {
        if(cooldownRemaining > 0)
        {
            cooldownRemaining--;
        }

        if (Input.GetKeyUp(key) && CanUse() && EnergyMeter.Instance().Deplete(energyCost))
        {   
            Activate();
            ResetCooldown();
        }
        MoveUpdate();
    }

    protected virtual void MoveUpdate() { }

    protected void ResetCooldown()
    {
        cooldownRemaining = cooldown;
    }

    //protected void Consume()
    //{
    //    consumed = true;
    //}

    protected bool CanUse()
    {
        return cooldownRemaining == 0;
        // return !consumed;
    }

    public abstract void Activate();
}
