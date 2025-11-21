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

    public float animationTime;

    public override void Activate()
    {        
        Player.Instance.playerAnim.SetTrigger("grab");
        StartCoroutine(LampAction());
    }
    protected override void MoveUpdate()
    {

    }
    protected override void MoveFixedUpdate()
    {

    }
    protected override bool CanUse()
    {       
        return Player.Instance.grounded && (cooldownRemaining <= 0 || isPlaced);
        // return !consumed;
    }

    protected override bool EscapeEnergyDeplete()
    {
        return isPlaced;
    }
    protected override void Initialize() 
    {
        
    }
    private IEnumerator LampAction()
    {
        Player.Instance.DisableMovementControl();
        yield return new WaitForSeconds(animationTime);
        if (isPlaced)
        {
            // Pick up the lamp if close enough, otherwise do nothing
            GameObject lamp = FindObjectOfType<LightPocketLamp>().gameObject;
            float dist = (lamp.transform.position - Player.Instance.transform.position).magnitude;
            if (dist <= pickupDistance)
            {
                Destroy(lamp);
                isPlaced = false;
            }

        }
        else
        {
            // Spawn a new lamp at the player's position, offset by placeDistance
            Vector3 pos = new Vector3(Player.Instance.transform.position.x + placeDistance.x * Player.Instance.dir, Player.Instance.transform.position.y + placeDistance.y, 0);
            Instantiate(pocketLampPrefab, pos, Quaternion.identity);
            isPlaced = true;
        }
        yield return new WaitForSeconds(animationTime);
        Player.Instance.EnableMovementControl();

    }

}
