using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    float patrolSpeed = 1;
    float runSpeed = 3;
    bool moveRight = false;
    Rigidbody2D rb;
    SpriteRenderer srC;
    Vector3 startingPos;
    Vector3 wanderPos;
    GameObject playerRefB;

    void Start()
    {
        SetEnemyDefaults(50, 15, 1, 5);
        OnStart();
        rb = GetComponent<Rigidbody2D>();
        srC = GetComponent<SpriteRenderer>();
        playerRefB = GameObject.FindGameObjectWithTag("Player");
        startingPos = transform.position;
        wanderPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        if (canSeePlayer)
        {
            speed = runSpeed;
            wanderPos = playerRefB.transform.position;
        }
        else
        {
            speed = patrolSpeed;
            // If at destination, pick a new one
            //Debug.Log(Vector3.Distance(transform.position, wanderPos));
            if(Vector3.Distance(transform.position, wanderPos) < 0.1)
            {
                wanderPos = new Vector3(startingPos.x + Random.Range(-1f, 1f), startingPos.y + Random.Range(-1f, 1f), 0);
            }
        }

        //rb.AddForce((wanderPos - transform.position).normalized * speed / 10);
        rb.velocity = new Vector2(wanderPos.x - transform.position.x, wanderPos.y - transform.position.y).normalized * speed;

        moveRight = rb.velocity.x > 0;

        UpdateSprite();
    }

    void UpdateSprite()
    {
        srC.flipX = moveRight;
    }
}
