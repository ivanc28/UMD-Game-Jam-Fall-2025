using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public GameObject lampOn;
    public GameObject placeLamp;
    public GameObject torchOn;
    public GameObject collectGem;
    public GameObject gemConnect;
    public GameObject gemConnect2;
    public GameObject powerupPlatform;

    private static SoundLibrary instance;
    public static SoundLibrary Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<SoundLibrary>();
            return instance;
        }
    }

}
