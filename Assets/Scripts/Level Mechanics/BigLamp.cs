using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLamp : MonoBehaviour
{
    const int GEM_COUNT = 3;
    private int curGemCount = 0;

    private Collider2D lampCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Gem>() != null)
        {
            collision.gameObject.SetActive(false);
            curGemCount++;
            Console.WriteLine("Added a gem");
        }

    }
}
