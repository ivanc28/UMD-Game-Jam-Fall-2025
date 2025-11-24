using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class BigLamp : MonoBehaviour
{
    public const int GEM_COUNT = 3;
    public int curGemCount { get; private set; } = 0;

    private Collider2D lampCollider;
    [SerializeField] SpriteRenderer platformSpriteRend;
    [SerializeField] Sprite activatedPlatform;
    [SerializeField] PlatformGem cGem1, cGem2, cGem3;
    bool gameOver = false;
    [SerializeField] float endGameDelay;
    [SerializeField] Light2D platformLight;
    float duration = 6.5f;
    float stopwatch = 0;
    float maxIntensity;
    float maxOuterRadius;
    bool canLightUp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        maxIntensity = platformLight.intensity * 1.75f;
        maxOuterRadius = platformLight.pointLightOuterRadius * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver && cGem1.SpriteEnabled() && cGem2.SpriteEnabled() && cGem3.SpriteEnabled())
        {            
            gameOver = true;
            StartCoroutine(DelayUpdateGems(endGameDelay));
            StartCoroutine(SoundPlayer.PlaySound(SoundLibrary.Instance.powerupPlatform, transform.position, 9));
        }

        if (gameOver && canLightUp)
        {
            stopwatch += Time.deltaTime;
            SetLightStrengthLerp(stopwatch / duration, maxIntensity, maxOuterRadius);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Gem>() != null)
        {
            Gem gem = collision.gameObject.GetComponent<Gem>();
            if(gem.id == 1)
            {
                cGem1.ActivateSpriteRenderer();
            }
            else if(gem.id == 2)
            {
                cGem2.ActivateSpriteRenderer();
            }
            else if(gem.id == 3)
            {
                cGem3.ActivateSpriteRenderer();
            }
            StartCoroutine(SoundPlayer.PlaySound(SoundLibrary.Instance.gemConnect, transform.position, 1));
            //StartCoroutine(SoundPlayer.PlaySound(SoundLibrary.Instance.gemConnect2, transform.position, 1));
            collision.gameObject.SetActive(false);
            curGemCount++;
            Console.WriteLine("Added a gem");
        }

    }

    private IEnumerator DelayUpdateGems(float delay)
    {
        yield return new WaitForSeconds(delay);
        cGem1.UpdateSpriteToActivated();
        cGem2.UpdateSpriteToActivated();
        cGem3.UpdateSpriteToActivated();
        platformSpriteRend.sprite = activatedPlatform;
        canLightUp = true;
        yield return new WaitForSeconds(9);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SetLightStrengthLerp(float interpolator, float maxIntensity, float maxOuterRadius)
    {
        platformLight.intensity = Mathf.Lerp(0, maxIntensity, interpolator);
        platformLight.pointLightOuterRadius = Mathf.Lerp(0, maxOuterRadius, interpolator);
    }
}
