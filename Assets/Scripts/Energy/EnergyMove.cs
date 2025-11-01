using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnergyMove : MonoBehaviour
{
    /// <summary>
    /// input key that triggers the move
    /// </summary>
    public KeyCode key;
    public int energyCost;
    /// <summary>
    /// How many ticks before move can be used again.
    /// </summary>
    public int cooldown;

    // TODO: Should we use cooldowns or a flag?
    protected int cooldownRemaining;
    
    // Start is called before the first frame update
    public void Start()
    {
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

    protected bool CanUse()
    {
        return cooldownRemaining == 0;
    }

    public abstract void Activate();
}
