using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnergyLamp : EnergyMove
{
    /// <summary>
    /// Lamp object that is placed next to the player
    /// </summary>
    public GameObject pocketLampPrefab;

    /// <summary>
    /// lamp is placed down
    /// </summary>
    private bool isPlaced;

    /// <summary>
    /// Distance in front of the player we will place the lamp
    /// </summary>
    public Vector2 placeDistance;

    /// <summary>
    /// Max distance player can pick up the lamp
    /// </summary>
    public float pickupDistance;

    public override void Activate()
    {
        if (isPlaced)
        {
            GameObject lamp = FindObjectOfType<LightPocketLamp>().gameObject;
            float dist = (lamp.transform.position - Player.Instance.transform.position).magnitude;
            Debug.Log(dist);
            if (dist <= pickupDistance)
            {
                Destroy(lamp);
                isPlaced = false;
            }
            
        }
        else
        {
            Debug.Log("A new lamp is born!");
            Vector3 pos = new Vector3(Player.Instance.transform.position.x + placeDistance.x * Player.Instance.dir, Player.Instance.transform.position.y + placeDistance.y, 0);
            Instantiate(pocketLampPrefab, pos, Quaternion.identity);
            isPlaced = true;
        }
    }
    protected override void MoveUpdate()
    {

    }
    protected override void MoveFixedUpdate()
    {

    }
    protected override bool CanUse()
    {
        return cooldownRemaining <= 0 || isPlaced;
        // return !consumed;
    }

    protected override bool CanDeactivate()
    {
        return isPlaced;
    }
    protected override void Initialize() 
    {
        
    }

}
