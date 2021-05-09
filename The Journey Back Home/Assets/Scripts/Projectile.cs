using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //How much damage the projectile does
    public int damage;
    //Whether or not the projectile is owned by the player
    public bool playerOwned;
    //Projectile death timer to not overload the game
    float death = 2;
    //Rigidbody for projectiles influenced by gravity
    Rigidbody2D rb;
    public float gravity = 0;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
    }

    // Update is called once per frame
    public void Update()
    {
        death -= Time.deltaTime;
        if(death <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
        if(collision.gameObject.layer == 9 && playerOwned)
        {
            //Damage the enemy
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Player" && !playerOwned)
        {
            //Damage the player
            collision.gameObject.GetComponent<PlayerData>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
