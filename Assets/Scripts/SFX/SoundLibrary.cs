using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public GameObject walkStone;

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
