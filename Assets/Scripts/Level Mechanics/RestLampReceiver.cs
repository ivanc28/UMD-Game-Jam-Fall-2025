using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestLampReceiver : LightActor
{
    public GameObject lights;
    public bool lightsOn;

    // Start is called before the first frame update
    public void Start()
    {
        lights.SetActive(false);
        lightsOn = false;
    }

    public override void Activate()
    {
        lights.SetActive(true);
        lightsOn = true;
    }

    public override void Deactivate()
    {
        lights.SetActive(false);
        lightsOn = false;
    }
}
