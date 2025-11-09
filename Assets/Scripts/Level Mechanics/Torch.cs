using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    /// <summary>
    /// Light component of the torch (child of object)
    /// </summary>
    [SerializeField] private Light2D lightComp;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lightComp.enabled = true;
            // TODO: Play lightup torch sound effect
        }
    }
}
