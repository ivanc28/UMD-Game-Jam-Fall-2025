using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPopups : MonoBehaviour
{
    public bool canQ = false, canE = true, canShift = false;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Sprite qIcon, eIcon, shiftIcon;

    private void Start()
    {
        rend.enabled = false;
        canE = true;
    }
    // Update is called once per frame
    void Update()
    {
        // super ugly but idc lol
        if(Player.Instance.dir == 1)
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }
        if (canQ)
        {
            rend.enabled = true;
            rend.sprite = qIcon;
            canE = false;
            canShift = false;
        }
        if (canE)
        {
            rend.enabled = true;
            rend.sprite = eIcon;
            canQ = false;
            canShift = false;
        }
        if (canShift)
        {
            rend.enabled = true;
            rend.sprite = shiftIcon;
            canE = false;
            canQ = false;
        }
        if(!canQ && !canE && !canShift)
        {
            rend.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            canShift = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            canQ= false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            canE = false;
        }
    }
}
