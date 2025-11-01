using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class EnergyMove : MonoBehaviour
{
    /// <summary>
    /// input key that triggers the move
    /// </summary>
    [Tooltip("Keycode that activates move")]
    public KeyCode key;

    [Tooltip("Energy consumed upon triggering move")]
    public int energyCost;


    /// <summary>
    /// How many ticks before move can be used again.
    /// </summary>
    [Tooltip("Cooldown in quantities of ticks")]
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
        //consumed = false;
        cooldownRemaining = 0;
        
        Initialize();
    }

    /// <summary>
    /// Called at start to grab component references
    /// </summary>
    protected virtual void Initialize() { }


    public void Update()
    {
        if (Input.GetKeyDown(key) && CanUse() && EnergyMeter.Instance().Deplete(energyCost))
        {
            ResetCooldown();
            Activate();
        }
    }

    public void FixedUpdate()
    {
        if(cooldownRemaining > 0)
        {
            cooldownRemaining--;
        }

        
        MoveUpdate();
    }

    /// <summary>
    /// Override method for inheriting classes to add their own code to update during FixedUpdate
    /// </summary>
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
