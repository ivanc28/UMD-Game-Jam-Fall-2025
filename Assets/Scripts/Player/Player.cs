using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public int dir;
    bool canMove;
    [HideInInspector] public bool canDash;
    [Header("Jumping")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float stopJumpSpeed;
    bool grounded;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask groundObjects;
    [SerializeField] Transform feetPos;
    [SerializeField] public float defaultGravity;
    [SerializeField] float fallingGravity;

    [Header("Coyote Time")]
    bool canCoyoteJump;
    [SerializeField] float setCoyoteTime;
    float coyoteTimer;

    private bool isRestoringEnergy = false;

    [Header("Components")]
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator playerAnim;
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
        playerAnim = GetComponent<Animator>();
        dir = 1;
        canMove = true;
        playerScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (canMove) {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
            // Set direction
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                SetDir(1);
                playerAnim.SetBool("isRunning", true);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                SetDir(-1);
                playerAnim.SetBool("isRunning", true);              
            }
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
            // Coyote Jump
            coyoteTimer = setCoyoteTime;
            canCoyoteJump = true;
            // Reset Dash
            canDash = true;
            canSetFallTrigger = false;
            playerAnim.SetBool("isFalling", false);
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if(!grounded && rb.velocity.y < 0)
        {
            if(canSetFallTrigger == false)
            {
                playerAnim.SetBool("isFalling", true);                
            }
            canSetFallTrigger = true;
        }

        if(coyoteTimer < 0 || rb.velocity.y > 0)
        {
            canCoyoteJump = false;
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
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnergyRestorer"))
        {
            isRestoringEnergy = false;
        }
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
    }
    // Player stops jumping
    void StopJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, stopJumpSpeed);
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
}
