using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAttractor : MonoBehaviour
{

    public Transform lampTransform;
    public bool finalAttractor = false;
    // Start is called before the first frame update
    void Start()
    {
        //if (lampTransform == null) lampTransform = GetComponentInChildren<Transform>();
        // Activate lights right away
        RestLampReceiver receiver = gameObject.GetComponent<RestLampReceiver>();
        if (receiver != null)
            gameObject.GetComponent<RestLampReceiver>().Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gem gem = collision.gameObject.GetComponent<Gem>();
        if (gem != null && gem.attractable)
        {
            if (gem.following == null)
            {                
                StartCoroutine(SoundPlayer.PlaySound(SoundLibrary.Instance.collectGem, transform.position, 1));
            }
            gem.following = lampTransform;
            if(finalAttractor)
            {
                gem.attractable = false;
            }
        }
    }
}
