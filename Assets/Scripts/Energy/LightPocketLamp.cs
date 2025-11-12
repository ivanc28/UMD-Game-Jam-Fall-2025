using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPocketLamp : LightSender
{
    /// <summary>
    /// maximum light duration
    /// </summary>
    public float maxDuration;
    private float duration;

    /// <summary>
    /// Collider component
    /// </summary>
    BoxCollider2D colComp;

    /// <summary>
    /// Collides with light to determine
    /// </summary>
    [SerializeField] CircleCollider2D effectiveLightCol;
    private float maxEffectRadius;

    public override void Init()
    {
        base.Init();
        colComp = GetComponent<BoxCollider2D>();
        effectiveLightCol = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(colComp, Player.Instance.GetComponent<Collider2D>());
        duration = maxDuration;
        maxEffectRadius = effectiveLightCol.radius;
    }

    public override void MakeUpdate()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            SetLightStrengthLerp(duration / maxDuration);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        LightReceiver receiver = collision.gameObject.GetComponent<LightReceiver>();
        if (receiver != null)
        {
            receiver.Intake(this);
        }
    }
    public override void SetLightStrengthLerp(float interpolator)
    {
        base.SetLightStrengthLerp(interpolator);
        effectiveLightCol.radius = Mathf.Lerp(0, maxEffectRadius, interpolator);
    }

    private void OnDrawGizmos()
    {
        // Light radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectiveLightCol.radius);
    }
}
