using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //A timer for how often this weapon can fire
    float timer = 0;
    public float rateOfFire;
    //How quick the projectile moves
    public float projectileSpeed;
    //How many projectiles are shot at once
    public int projectileAmount;
    //How spread out the projectiles are -- for one projectile, this should be 0
    public float spread = 0;
    //The projectile prefab
    public GameObject projectile;
    //Face the direction the mouse is pointing
    Vector2 direction;
    //Attach ourselves to the player
    PlayerMovement player;

    SpriteRenderer sr;
    AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    public void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        timer -= Time.deltaTime;
        //Attach to the player
        this.transform.position = player.transform.position;
        //Look to where the mouse is
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Flip the sprite if the gun is facing left
        sr.flipY = angle > 90 || angle < -90;

        //Wait for player to click
        if (Input.GetAxisRaw("Fire1") > 0 && timer <= 0)
        {
            //Debug.Log("We're shootin'!");
            //Reset timer
            timer = rateOfFire;
            for(int i = 0; i < projectileAmount; i++)
            {
                //Create a new bullet
                GameObject temp = Instantiate(projectile, this.gameObject.transform.position, this.gameObject.transform.rotation);
                //Shoot the bullet in the way we're facing
                temp.GetComponent<Rigidbody2D>().velocity = this.transform.right * projectileSpeed;
                //Spread the bullet(s) out -- a faster projectile speed makes this more difficult to tell
                temp.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread)));
                //Play Audio
                audioSource.PlayOneShot(clip);
            }
        }
    }


}
