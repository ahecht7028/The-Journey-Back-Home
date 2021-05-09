using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : Enemy
{
    float patrolSpeed = 1;
    float runSpeed = 3;
    bool moveRight = false;
    Rigidbody2D rb;
    SpriteRenderer srC;

    void Start()
    {
        SetEnemyDefaults(40, 10, 1, 5);
        OnStart();
        rb = GetComponent<Rigidbody2D>();
        srC = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        OnUpdate();
        if (canSeePlayer)
        {
            speed = runSpeed;
        }
        else
        {
            speed = patrolSpeed;
        }

        if((IsTouchingLeftEdge() || IsTouchingLeftWall()) && !moveRight)
        {
            moveRight = true;
        }
        if((IsTouchingRightEdge() || IsTouchingRightWall()) && moveRight)
        {
            moveRight = false;
        }

        if (moveRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        UpdateSprite();
    }


    // Collisions
    bool IsTouchingLeftWall()
    {
        Collider2D[] leftCollision = new Collider2D[2];

        int numOfCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x - 0.15f, transform.position.y - -0.15f), new Vector2(0.02f, 0.1f), 0.0f, leftCollision, layerMask);
        for (int i = 0; i < numOfCollisions; i++)
        {
            if (!ReferenceEquals(gameObject, leftCollision[i].gameObject))
            {
                return true;
            }
        }
        return false;
    }

    bool IsTouchingRightWall()
    {
        Collider2D[] rightCollision = new Collider2D[2];

        int numOfCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x + 0.15f, transform.position.y - -0.15f), new Vector2(0.02f, 0.1f), 0.0f, rightCollision, layerMask);
        for (int i = 0; i < numOfCollisions; i++)
        {
            if (!ReferenceEquals(gameObject, rightCollision[i].gameObject))
            {
                return true;
            }
        }
        return false;
    }

    bool IsTouchingLeftEdge()
    {
        bool noGround = true;

        Collider2D[] leftEdge = new Collider2D[2];

        int numOfCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x - 0.15f, transform.position.y - 0.3f), new Vector2(0.05f, 0.05f), 0.0f, leftEdge, layerMask);
        for (int i = 0; i < numOfCollisions; i++)
        {
            if (!ReferenceEquals(gameObject, leftEdge[i].gameObject))
            {
                noGround = false;
            }
        }

        return noGround;
    }

    bool IsTouchingRightEdge()
    {
        bool noGround = true;

        Collider2D[] rightEdge = new Collider2D[2];

        int numOfCollisions = Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x + 0.15f, transform.position.y - 0.3f), new Vector2(0.05f, 0.05f), 0.0f, rightEdge, layerMask);
        for (int i = 0; i < numOfCollisions; i++)
        {
            if (!ReferenceEquals(gameObject, rightEdge[i].gameObject))
            {
                noGround = false;
            }
        }
        
        return noGround;
    }

    void UpdateSprite()
    {
        srC.flipX = moveRight;
    }
}
