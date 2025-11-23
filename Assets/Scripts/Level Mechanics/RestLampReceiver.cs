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
        // Don't play the sound for the big lamp/light house
        if (gameObject.GetComponent<GemAttractor>() == null)
        {
            StartCoroutine(SoundPlayer.PlaySound(SoundLibrary.Instance.lampOn, transform.position, 0.25f));
        }
    }

    public override void Deactivate()
    {
        lights.SetActive(false);
        lightsOn = false;
    }
}
