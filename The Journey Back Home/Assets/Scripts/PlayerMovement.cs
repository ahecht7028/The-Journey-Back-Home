using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    public float jumpSpeed;

    bool faceRight;
    bool slideOnRight;
    bool canMove;
    public bool isDead = false;
    LayerMask groundLayer;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    void Start()
    {
        faceRight = true;
        slideOnRight = true;
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        if (canMove && !isDead)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
            animator.SetFloat("MovementX", x);

            if (IsGrounded())
            {
                rb.velocity = new Vector2(x, rb.velocity.y);
            }
            else if (rb.velocity.x > Time.deltaTime * playerSpeed && x < 0)
            {
                rb.AddForce(new Vector2(x * 5, 0));
            }
            else if (rb.velocity.x < -Time.deltaTime * playerSpeed && x > 0)
            {
                rb.AddForce(new Vector2(x * 5, 0));
            }
            else if (rb.velocity.x <= Time.deltaTime * playerSpeed && rb.velocity.x >= -Time.deltaTime * playerSpeed)
            {
                rb.AddForce(new Vector2(x * 5, 0));
            }
            if (x > 0)
            {
                faceRight = true;
                UpdateSprite();
            }
            if (x < 0)
            {
                faceRight = false;
                UpdateSprite();
            }
        }
    }

    void Update()
    {
        // Wall Slide
        if (IsWallSliding())
        {
            if(rb.velocity.y > 0)
            {
                rb.gravityScale = 1.0f;
            }
            else
            {
                rb.gravityScale = 0.1f;
            }

            // Check for wall jump
            if (Input.GetKeyDown(KeyCode.W) && !isDead)
            {
                if (slideOnRight)
                {
                    rb.velocity = new Vector2(-jumpSpeed / 50, jumpSpeed / 50);
                }
                else
                {
                    rb.velocity = new Vector2(jumpSpeed / 50, jumpSpeed / 50);
                }
            }
        }
        else
        {
            rb.gravityScale = 1.0f;

            // Jump Movement
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded() && !isDead)
            {
                rb.AddForce(new Vector2(0, jumpSpeed));
            }
        }

        animator.SetFloat("MovementY", rb.velocity.y);
    }

    bool IsGrounded()
    {
        Collider2D[] collisions = new Collider2D[2];
        // Checks for collisions made under the player
        // The "GroundCheckSprite" child object is a collider with the same position and size to check for visual purposes.
        int numOfCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x, transform.position.y - 0.4f), new Vector2(0.2f, 0.2f), 0.0f, collisions, groundLayer);
        for(int i = 0; i < numOfCollisions; i++)
        {
            if(!ReferenceEquals(gameObject, collisions[i].gameObject))
            {
                animator.SetBool("Grounded", true);
                return true;
            }
        }
        animator.SetBool("Grounded", false);
        return false;
    }

    bool IsWallSliding()
    {
        if (!IsGrounded())
        {
            // Have to check both left and right sides
            Collider2D[] leftCollisions = new Collider2D[2];
            Collider2D[] rightCollisions = new Collider2D[2];
            int numOfLeftCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x - 0.2f, transform.position.y), new Vector2(0.2f, 0.2f), 0.0f, leftCollisions, groundLayer);
            int numOfRightCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x + 0.2f, transform.position.y), new Vector2(0.2f, 0.2f), 0.0f, rightCollisions, groundLayer);

            // Check if sliding on left wall
            for (int i = 0; i < numOfLeftCollisions; i++)
            {
                if (!ReferenceEquals(gameObject, leftCollisions[i].gameObject))
                {
                    slideOnRight = false;
                    animator.SetBool("WallSliding", true);
                    return true;
                }
            }

            // Check if sliding on right wall
            for (int i = 0; i < numOfRightCollisions; i++)
            {
                if (!ReferenceEquals(gameObject, rightCollisions[i].gameObject))
                {
                    slideOnRight = true;
                    animator.SetBool("WallSliding", true);
                    return true;
                }
            }
        }
        animator.SetBool("WallSliding", false);
        return false;
    }

    public void DamageKnockback()
    {
        if (faceRight)
        {
            rb.velocity = new Vector2(-5, 5);
            transform.position += new Vector3(0, 0.25f, 0);
        }
        else
        {
            rb.velocity = new Vector2(5, 5);
            transform.position += new Vector3(0, 0.25f, 0);
        }

        canMove = false;
    }

    void UpdateSprite()
    {
        sr.flipX = !faceRight;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Lets the player move after being hit to the ground
        if (collision.gameObject.layer == 8)
        {
            canMove = true;
        }
    }
}
