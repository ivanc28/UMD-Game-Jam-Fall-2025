using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

/// <summary>
/// Moving Platform that is triggered by a lightreceiver
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformActor : LightActor
{
    public Vector3 finalPosition;
    public float speed;
    public Light2D activatedLight;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private AudioSource movingSFX;
    private float initVolume;

    public void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        movingSFX = GetComponent<AudioSource>();
        rb.isKinematic = true;
        targetPosition = initialPosition;
        initVolume = movingSFX.volume;

    }

    public void FixedUpdate() 
    {
        if (rb.velocity.magnitude > 0)
        {
            // stop rb from overshooting
            if (PlatformOvershot()) 
            {
                SetPosToFinal();
            }            
            TryToPlaySFX();
        }
    }

    public override void Activate()
    {
        activatedLight.enabled = true;
        if (PlatformOvershot())
        {
            SetPosToFinal();
            return;
        }
        Vector3 vel = (finalPosition - transform.position).normalized * speed;
        targetPosition = finalPosition;
        rb.velocity = vel;
    }

    public override void Deactivate()
    {
        activatedLight.enabled = false;
        Vector3 vel = (initialPosition - targetPosition).normalized * speed;
        targetPosition = initialPosition;
        rb.velocity = vel;
    }
    private bool PlatformOvershot()
    {
        // if the direction to initial is same as to final, the object must have overshot
        return Vector3.Dot(targetPosition - transform.position, rb.velocity) < 0;
    }
    private void SetPosToFinal()
    {
        transform.position = targetPosition;
        rb.velocity = Vector2.zero;
        StartCoroutine(FadeOutSFX(movingSFX, 0.35f));
    }
    private void TryToPlaySFX()
    {
        if (!movingSFX.isPlaying)
        {
            if (activatedLight.enabled)
            {
                movingSFX.volume = initVolume;
            }
            else
            {
                movingSFX.volume = initVolume / 2;
            }
            movingSFX.pitch = Random.Range(0.9f, 1.1f);
            movingSFX.Play();
        }
    }
    IEnumerator FadeOutSFX(AudioSource audioSource, float duration)
    {  
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = initVolume;
    }
}
