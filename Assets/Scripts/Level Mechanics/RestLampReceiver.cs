using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestLampReceiver : LightReceiver
{
    public GameObject lights;
    public bool lightsOn;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        lightTreshold = Player.Instance.GetComponent<EnergyRest>().energyCost;
        lights.SetActive(false);
        lightsOn = false;
    }

    public override void Activate()
    {
        lights.SetActive(true);
        lightsOn = true;
    }
}
