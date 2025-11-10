using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSender : MonoBehaviour
{
    /// <summary>
    /// disables ability to send light to receivers
    /// </summary>
    public bool disableLightSend;

    [SerializeField] private Light2D lightComp;
    private float maxIntensity;
    private float maxOuterRadius;
    private float maxInnerRadius;

    /// <summary>
    /// light value of projectile. Used for accumulating light in light receivers.
    /// </summary>
    public int maxLightValue;
    private int currLightValue;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        MakeUpdate();
    }
    /// <summary>
    /// Returns the current light value
    /// </summary>
    /// <returns>current light value</returns>
    public int GetLightValue()
    {
        return currLightValue;
    }

    /// <summary>
    /// Sets the current light value to the specified value if less than max light value. Otherwise, sets to max light value
    /// </summary>
    /// <param name="value"></param>
    public void SetLightValue(int value)
    {
        currLightValue = value > maxLightValue ? maxLightValue : value;
    }

    /// <summary>
    /// Sets the light's size and intensity to a value between 0 and the corresponding max, determined by the interpolator (float from 0 to 1)
    /// </summary>
    /// <param name="interpolator"></param>
    public virtual void SetLightStrengthLerp(float interpolator)
    {
        lightComp.intensity = Mathf.Lerp(0, maxIntensity, interpolator);
        lightComp.pointLightOuterRadius = Mathf.Lerp(0, maxOuterRadius, interpolator);
        lightComp.pointLightInnerRadius = Mathf.Lerp(0, maxInnerRadius, interpolator);
        SetLightValue((int)Mathf.Round(Mathf.Lerp(0, maxLightValue, interpolator)));
    }

    /// <summary>
    /// Initializes the object (called in Start)
    /// </summary>
    public virtual void Init()
    {
        if (!disableLightSend)
        {
            currLightValue = maxLightValue;
            maxIntensity = lightComp.intensity;
            maxOuterRadius = lightComp.pointLightOuterRadius;
            maxInnerRadius = lightComp.pointLightInnerRadius;
        }
    }

    public virtual void MakeUpdate()
    {

    }
}
