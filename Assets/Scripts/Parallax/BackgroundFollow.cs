using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundFollow : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }


    void LateUpdate()
    {
        //if(Player.Instance.rb.velocity.y > 0)
        //{
            
        //}
        transform.position = new Vector3(transform.position.x, cam.position.y, transform.position.z);
    }
}
