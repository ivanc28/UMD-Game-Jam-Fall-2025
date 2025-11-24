using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Gem : MonoBehaviour
{
    [Tooltip("How long until the gem will unstick itself by teleporting to the following transform")]
    public float stuckTimer = 5;
    public float stuckRadius = 20;

    public float attractStrength = 5;
    public float repelGemRadius = 1f;
    public float repelRadius = 2;

    [Tooltip("max distance from repel radius where the gem will not change it's velocity")]
    public float stayPadding = 0.5f;

    public float repelStrength = 10;

    public Transform following = null;

    public bool attractable = true;

    public Gem[] otherGems = new Gem[2];


    private float timer = 0;
    private bool possibleStuck = false;
    private Vector2 direction;
    private Vector3 displacement;
    private Rigidbody2D rb;
    private Collider2D coll;

    public int id;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        otherGems = FindObjectsOfType<Gem>();
    }

    void UnStuckify()
    {
        if (displacement.magnitude > stuckRadius)
        {
            if (!possibleStuck)
            {
                possibleStuck = true;
                timer = 0;
            }
        }
        else
        {
            possibleStuck = false;
        }
        if (possibleStuck)
        {
            timer += Time.deltaTime;
            if (timer >= stuckTimer)
            {
                possibleStuck = false;
                timer = 0;
                transform.position = following.position + (displacement.normalized * -1);
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        if(following != null)
        {
            displacement = (following.position - transform.position);
            direction = displacement.normalized;

            float distance = displacement.magnitude;

            if(distance > repelRadius + stayPadding)
            {
                velocity = direction * (distance - repelRadius) * attractStrength;

            }
            else
            {
                if (distance < repelRadius)
                {
                    velocity = -direction * repelStrength;
                }
            }

            foreach (Gem gem in otherGems)
            {
                if (gem != this)
                {
                    Vector3 gemPos = gem.transform.position;
                    Vector3 displacement = transform.position - gemPos;
                    if (displacement.magnitude < repelGemRadius)
                    {
                        velocity += displacement;
                    }
                }
            }

            rb.velocity = velocity;

            UnStuckify();
        }
        
        

    }
}
