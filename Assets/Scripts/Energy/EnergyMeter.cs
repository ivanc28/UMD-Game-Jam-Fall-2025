using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
/// EnergyMeter singleton. May not need to be singleton.
/// </summary>
public class EnergyMeter : MonoBehaviour
{
    /// <summary>
    /// current energy value held by player
    /// </summary>
    public int curEnergy;
    /// <summary>
    /// max energy a player can hold
    /// </summary>
    public int maxEnergy;
    /// <summary>
    /// how much energy depletes per tick
    /// </summary>
    public int energyDecay;
    /// <summary>
    /// how much energy regenerates per tick
    /// </summary>
    public int energyRecovery;

    static EnergyMeter instance;
    /// <summary>
    /// Grabs instance of EnergyMeter from the current scene
    /// </summary>
    /// <returns>Scene's first found EnergyMeter object</returns>
    public static EnergyMeter Instance()
    {
        if(instance == null)
        {
            instance = FindFirstObjectByType<EnergyMeter>();
        }
        else
        {
            Debug.LogError("You do not have an EnergyMeter class instance in this scene!");
        }
        return instance;
    }

    /// <summary>
    /// EnergyMeter Constructor. Is this needed?
    /// </summary>
    /// <param name="maxEnergy"></param>
    /// <param name="energyDecay"></param>
    /// <param name="energyRecovery"></param>
    public EnergyMeter(int maxEnergy, int energyDecay, int energyRecovery)
    {
        this.maxEnergy = maxEnergy;
        this.energyDecay = energyDecay;
        this.energyRecovery = energyRecovery;
        curEnergy = maxEnergy;
    }

    /// <summary>
    /// Depletes energy by energyDecay. This is intended to be called during FixedUpdate.
    /// </summary>
    public void Decay()
    {
        if (curEnergy > 0)
        {
            curEnergy -= energyDecay;
        }
        else if (curEnergy < 0)
        {
            curEnergy = 0;
        }
    }

    /// <summary>
    /// Replenishes energy by energyRecovery. This is intended to be called during FixedUpdate.
    /// </summary>
    public void Recover()
    {
        if (curEnergy < maxEnergy)
        {
            curEnergy += energyRecovery;
        }
        else if (curEnergy > maxEnergy)
        {
            curEnergy = maxEnergy;
        }
    }

    /// <summary>
    /// Reduces curEnergy by amount. Returns whether depletion was successful.
    /// </summary>
    /// <param name="amount">amount of energy to deplete</param>
    /// <returns>true if curEnergy is greater than or equal to amount, <br></br> false if curEnergy is less than amount </returns>
    public bool Deplete(int amount)
    {
        if (curEnergy > amount)
        {
            curEnergy -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
