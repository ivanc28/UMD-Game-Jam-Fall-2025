using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    int dir = 1;
    [Header("Jumping")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float stopJumpSpeed;
    bool grounded;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask groundObjects;
    [SerializeField] Transform feetPos;
    [SerializeField] float defaultGravity;
    [SerializeField] float fallingGravity;
    [Header("Coyote Time")]
    bool canCoyoteJump;
    [SerializeField] float setCoyoteTime;
    float coyoteTimer;

    [Header("Components")]
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        coyoteTimer = setCoyoteTime;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
        // Set direction
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetDir(1);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetDir(-1);
        }

        // On the ground if player's feet is touching object with 'Ground' layer
        grounded = Physics2D.OverlapCircle(feetPos.position, circleRadius, groundObjects);
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || canCoyoteJump))
        {
            Jump();
        }
        // Stop jump when let go of space
        if((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && rb.velocity.y > stopJumpSpeed)
        {
            StopJump();
        }

        // Adjust gravity scale
        if(rb.velocity.y > 0)
        {
            SetGravityScale(defaultGravity);
        }
        else
        {
            SetGravityScale(fallingGravity);
        }

        // Coyote Jump
        if(grounded)
        {
            coyoteTimer = setCoyoteTime;
            canCoyoteJump = true;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if(coyoteTimer < 0 || rb.velocity.y > 0)
        {
            canCoyoteJump = false;
        }
    }

    // Collision detection (isTriggers)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
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
    void SetGravityScale(float gravityScale)
    {
        rb.gravityScale = gravityScale;
    }
}
