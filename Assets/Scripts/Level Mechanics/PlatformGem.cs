using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGem : MonoBehaviour
{
    private SpriteRenderer rend;
    [SerializeField] Sprite activatedSprite;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SpriteEnabled()
    {
        return rend.enabled;
    }

    public void ActivateSpriteRenderer()
    {
        rend.enabled = true;
    }

    public void UpdateSpriteToActivated()
    {
        rend.sprite = activatedSprite;
    }
}
