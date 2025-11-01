using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyTracker : MonoBehaviour
{
    Slider slider;
    EnergyMeter energy;

    public void Start()
    {
        slider = GetComponent<Slider>();
        energy = EnergyMeter.Instance();
        slider.maxValue = energy.maxEnergy;

    }

    public void Update()
    {
        slider.value = energy.curEnergy;
    }

}
