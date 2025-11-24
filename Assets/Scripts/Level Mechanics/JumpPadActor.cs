using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class JumpPadActor : LightActor
{
    public float jumpStrength;
    public Light2D activatedLight;
    bool isActive;
    private AudioSource onSFX;
    public AudioSource bounceSFX;
    bool canStopOnSFX = true;
    private void Start()
    {
        isActive = false;
        onSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Sound Effect
        if (isActive)
        {
            if (!onSFX.isPlaying)
            {
                onSFX.Play();
                canStopOnSFX = true;
            }
        }
        else
        {
            if (canStopOnSFX)
            {
                StartCoroutine(FadeOutSFX(onSFX, 0.5f));
                canStopOnSFX = false;
            }
        }
    }
    public override void Activate()
    {
        isActive = true;
        activatedLight.enabled = true;
    }

    public override void Deactivate()
    {
        isActive = false;
        activatedLight.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        UseJumpPad(collision);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        UseJumpPad(collision);
    }

    private void UseJumpPad(Collider2D collision)
    {
        if (isActive)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Vector3 plrVel = player.GetComponent<Rigidbody2D>().velocity;
                plrVel.y = jumpStrength;
                player.GetComponent<Rigidbody2D>().velocity = plrVel;
                player.playerAnim.SetBool("isFalling", false);
                player.canSetFallTrigger = true;
                player.playerAnim.SetTrigger("jump");
                bounceSFX.Play();
            }
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
        audioSource.volume = startVolume;
    }

}
