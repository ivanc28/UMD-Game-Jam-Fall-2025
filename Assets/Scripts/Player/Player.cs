using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public int dir;
    [SerializeField] bool canMove;
    public bool canDash;
    private Vector2 movingPlatformSpeed;
    private bool onMovingPlatform = false;
    public bool dashAcquired = false;
    [Header("Jumping")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float stopJumpSpeed;
    [HideInInspector] public bool grounded;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask groundObjects;
    [SerializeField] Transform feetPos;
    [SerializeField] public float defaultGravity;
    [SerializeField] float fallingGravity;
    private bool isJumping;

    [Header("Pocket Lamp")]
    [HideInInspector] public bool hasLamp = true;

    [Header("Coyote Time")]
    bool canCoyoteJump;
    [SerializeField] float setCoyoteTime;
    float coyoteTimer;

    [HideInInspector] public bool isRestoringEnergy = false;

    [Header("Sound Effects")]
    [SerializeField] AudioSource walkSFX;
    [SerializeField] AudioClip[] walkSoundClips;
    [SerializeField] AudioSource energyRestoreSFX;
    bool canStopEnergySFX = true;
    [SerializeField] AudioSource jumpSFX;

    [Header("Components")]
    public Animator playerAnim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool canSetFallTrigger;
    Vector3 playerScale;

    // Singleton
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        coyoteTimer = setCoyoteTime;
        rb = GetComponent<Rigidbody2D>();
        dir = 1;
        canMove = true;
        playerScale = transform.localScale;
        movingPlatformSpeed = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (canMove) {
            if (onMovingPlatform)
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed + movingPlatformSpeed.x, rb.velocity.y);
            }
            else if(!onMovingPlatform)
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
            }
            // Set direction
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                SetDir(1);
                playerAnim.SetBool("isRunning", true);
                if (grounded)
                {
                    PlayWalkSFX();
                }
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                SetDir(-1);
                playerAnim.SetBool("isRunning", true);
                if (grounded)
                {
                    PlayWalkSFX();
                }
            }
        }
        else if(!GetComponent<EnergyDash>().dashing)
        {
            rb.velocity = new Vector2(movingPlatformSpeed.x, rb.velocity.y);
        }
        if (Input.GetAxisRaw("Horizontal") == 0 || !canMove)
        {
            playerAnim.SetBool("isRunning", false);
        }
        // Flip player sprite direction
        transform.localScale = new Vector3(dir * playerScale.x, playerScale.y, playerScale.z);

        // On the ground if player's feet is touching object with 'Ground' layer
        grounded = Physics2D.OverlapCircle(feetPos.position, circleRadius, groundObjects);
        if (canMove)
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && (grounded || canCoyoteJump))
            {
                Jump();
            }
            // Stop jump when let go of space
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && rb.velocity.y > stopJumpSpeed)
            {
                StopJump();
            }
        }

        // Adjust gravity scale
        if(canMove)
        {
            if (rb.velocity.y > 0)
            {
                SetGravityScale(defaultGravity);
            }
            else
            {
                SetGravityScale(fallingGravity);
            }
        }

        if(grounded)
        {
            isJumping = false;
            // Coyote Jump
            coyoteTimer = setCoyoteTime;
            canCoyoteJump = true;
            // Reset Dash
            canDash = true;
            canSetFallTrigger = true;
            playerAnim.SetBool("isFalling", false);
            playerAnim.SetBool("grounded", true);
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            playerAnim.SetBool("grounded", false);
        }
        if(!grounded && rb.velocity.y < 0)
        {
            if(canSetFallTrigger == true)
            { 
                StartCoroutine(StartFallAnimAfterDelay(0.1f));   
            }
            canSetFallTrigger = false;
        }

        if(coyoteTimer < 0 || rb.velocity.y > 0)
        {
            canCoyoteJump = false;
        }

        // Energy restore sfx
        if (isRestoringEnergy)
        {
            if (!energyRestoreSFX.isPlaying)
            {
                energyRestoreSFX.Play();
                canStopEnergySFX = true;
            }
        }
        else
        {
            if (energyRestoreSFX.isPlaying)
            {
                if (canStopEnergySFX)
                {
                    StartCoroutine(FadeOutSFX(energyRestoreSFX, 0.25f));
                    canStopEnergySFX = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRestoringEnergy)
        {
            EnergyMeter.Instance().Recover();
        }
    }

    // Collision detection (isTriggers)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnergyRestorer"))
        {
            isRestoringEnergy = true;
        }
        if (collision.gameObject.CompareTag("DashAbility"))
        {
            dashAcquired = true;
            Destroy(collision.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnergyRestorer"))
        {
            isRestoringEnergy = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            StartCoroutine(AttachToPlatform(collision.transform));
            transform.localScale = new Vector3(playerScale.x * dir, playerScale.y, playerScale.z);
            movingPlatformSpeed = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            onMovingPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
            transform.localScale = new Vector3(playerScale.x * dir, playerScale.y, playerScale.z);
            movingPlatformSpeed = Vector2.zero;
            onMovingPlatform = false;
        }
    }

    IEnumerator AttachToPlatform(Transform platform)
    {
        yield return new WaitForEndOfFrame(); ; // wait 1 frame to avoid activation conflict
        transform.SetParent(platform);
    }

    // View Gizmos in editor
    private void OnDrawGizmosSelected()
    {
        // Can view the collider of the player's ground detection
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetPos.position, circleRadius);
    }
    // Player jumps
    void Jump()
    {
        rb.velocity = Vector2.up * jumpSpeed;
        isJumping = true;
        PlayJumpSFX();
        // Animations
        playerAnim.SetBool("isFalling", false);
        canSetFallTrigger = true;
        playerAnim.SetTrigger("jump");
        StartCoroutine(ResetAnim(0.5f, 0.01f));
    }
    // Player stops jumping
    void StopJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, stopJumpSpeed);
        isJumping = false;
    }
    // Set the direction the player is facing
    void SetDir(int direction)
    {
        dir = direction;
    }
    // Set the gravity of the player
    public void SetGravityScale(float gravityScale)
    {
        rb.gravityScale = gravityScale;
    }

    public void EnableMovementControl()
    {
        canMove = true;
    }

    public void DisableMovementControl()
    {
        canMove = false;
    }

    // Sounds
    private void PlayWalkSFX()
    {
        if (!walkSFX.isPlaying)
        {
            walkSFX.clip = walkSoundClips[UnityEngine.Random.Range(0, walkSoundClips.Length)];
            walkSFX.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            walkSFX.Play();
        }
    }

    private void PlayJumpSFX()
    {
        jumpSFX.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
        jumpSFX.Play();
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

    // Animations
    private IEnumerator StartFallAnimAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rb.velocity.y < 0)
        {
            playerAnim.SetBool("isFalling", true);
            yield break;
        }
        StartCoroutine(ResetAnim(0f, 0.2f));
    }
    private IEnumerator ResetAnim(float delayToResetAnim, float delayToResetTrigger)
    {
        yield return new WaitForSeconds(delayToResetAnim);
        playerAnim.SetTrigger("reset");
        if (isJumping)
        {
            playerAnim.ResetTrigger("reset");
        }
        yield return new WaitForSeconds(delayToResetTrigger);
        playerAnim.ResetTrigger("reset");
    }
 

}
