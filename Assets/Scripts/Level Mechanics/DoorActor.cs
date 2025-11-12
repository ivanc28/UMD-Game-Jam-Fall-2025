using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DoorActor : LightActor
{

    
    /// <summary>
    /// (UNIMPLEMENTED) <br></br>
    /// This is optional, but if you want to know when a player passes through the door, you can pass an action.<br></br>
    /// This has limited functionality, cannot pass parameters.
    /// </summary>
    [Tooltip("(UNIMPLEMENTED) Script to run when the player passes through the door. Doesn't do anything if null")]
    public UnityAction callBack;

    private Collider2D col;
    private SpriteRenderer sprite;
    /// <summary>Start is called before the first frame update</summary> 
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public override void Activate()
    {
        col.isTrigger = true;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
    }

    public override void Deactivate()
    {
        col.isTrigger = false;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);

    }


    //Not working but also not needed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            callBack?.Invoke();
        }
    }
}
