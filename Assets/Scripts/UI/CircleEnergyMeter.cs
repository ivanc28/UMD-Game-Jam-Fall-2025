using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnergyMeter : MonoBehaviour
{
    public GameObject energymeterObj;

    private EnergyMeter energy;
    private float maxEnergy;
    private float curEnergy;
    private Vector3 maxSize;
    // Start is called before the first frame update
    void Start()
    {
        energy = EnergyMeter.Instance();
        maxEnergy = energy.maxEnergy;
        maxSize = energymeterObj.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        curEnergy = energy.curEnergy;
        energymeterObj.transform.localScale = maxSize * (curEnergy / maxEnergy);
    }
}
