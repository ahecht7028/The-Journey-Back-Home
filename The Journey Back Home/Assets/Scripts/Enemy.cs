using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    int health;
    int maxHealth;
    int damage;
    [HideInInspector]
    public float speed;
    float seeDistance;
    GameObject playerRef;
    RaycastHit2D rayHit;
    [HideInInspector]
    public int layerMask;
    [HideInInspector]
    public int enemyLayerMask;
    SpriteRenderer sr;
    [HideInInspector]
    public bool canSeePlayer;

    public void OnStart()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        layerMask = 1 << 8; // Layer 8 is Ground Layer
        enemyLayerMask = 1 << 9; // Layer 9 is Enemy Layer
        canSeePlayer = false;
    }

    public void OnUpdate()
    {
        // Enemy Vision
        rayHit = Physics2D.Linecast(transform.position, playerRef.transform.position, layerMask);
        canSeePlayer = !rayHit && Vector2.Distance(transform.position, playerRef.transform.position) < seeDistance;
        /*
        if (canSeePlayer)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.green;
        }
        */
    }

    // Sets all the values of the abstract class that are needed
    public void SetEnemyDefaults(int _health, int _damage, float _speed, float _seeDistance)
    {
        health = _health;
        maxHealth = _health;
        damage = _damage;
        speed = _speed;
        seeDistance = _seeDistance;
    }

    // Health Methods
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        CheckHealth();
        StartCoroutine(Flash(Color.red));
    }

    public void Heal(int _heal)
    {
        health += _heal;
        CheckHealth();
        StartCoroutine(Flash(Color.green));
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            KillEnemy();
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    IEnumerator Flash(Color color)
    {
        for (int i = 0; i < 50; i++)
        {
            sr.color = color;
            yield return new WaitForSeconds(0.01f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void KillEnemy()
    {
        // Kill the Enemy
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerData>().TakeDamage(damage);
            //Debug.Log("Touched Player");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerData>().TakeDamage(damage);
            //Debug.Log("Touched Player");
        }
    }
}
