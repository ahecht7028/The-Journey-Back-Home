using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static int level = 1;

    int health;
    int maxHealth;
    bool invulnerable;
    SpriteRenderer sr;
    PlayerMovement playerm;
    Image healthBar;
    public GameObject[] weapons;
    Text gameOverText;
    AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip weaponPickupSound;

    void Start()
    {
        health = 100;
        maxHealth = 100;
        invulnerable = false;
        sr = GetComponent<SpriteRenderer>();
        playerm = GetComponent<PlayerMovement>();
        healthBar = GameObject.Find("Canvas").transform.Find("HealthBar").transform.Find("HealthBarFill").GetComponent<Image>();
        gameOverText = GameObject.Find("Canvas").transform.Find("GameOver").GetComponent<Text>();
        gameOverText.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            TakeDamage(5);
        }

        // If the player falls off the level, it kills them
        if(transform.position.y < -15f)
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int _damage)
    {
        if (!invulnerable)
        {
            health -= _damage;
            CheckHealth();
            if(health > 0)
            {
                StartCoroutine(Flash(Color.red, false));
                playerm.DamageKnockback();
                StartCoroutine(InvulnerableTime());
                audioSource.PlayOneShot(hurtSound);
            }
        }
    }

    public void Heal(int _heal)
    {
        health += _heal;
        StartCoroutine(Flash(Color.green, false));
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateUI();
    }

    IEnumerator Flash(Color color, bool invisible)
    {
        if (invisible)
        {
            for(int i = 0; i < 50; i++)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
                yield return new WaitForSeconds(0.01f);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (int i = 0; i < 50; i++)
            {
                sr.color = color;
                yield return new WaitForSeconds(0.01f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.01f);
            }
            if (color == Color.red)
            {
                StartCoroutine(Flash(color, true));
            }
        }
    }

    IEnumerator InvulnerableTime()
    {
        invulnerable = true;
        yield return new WaitForSeconds(2f);
        invulnerable = false;
    }

    void Die()
    {
        invulnerable = true;
        sr.enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (FindObjectOfType<Weapon>() != null)
        {
            Destroy(FindObjectOfType<Weapon>().gameObject);
        }
        GetComponent<BoxCollider2D>().enabled = false;
        playerm.isDead = true;
        gameOverText.enabled = true;
        FindObjectOfType<CameraController>().isDead = true;
        StartCoroutine(TitleScreenTransition());
    }

    void UpdateUI()
    {
        healthBar.fillAmount = (float)health / maxHealth;
    }

    void EquipWeapon(int id)
    {
        if(FindObjectOfType<Weapon>() != null)
        {
            Destroy(FindObjectOfType<Weapon>().gameObject);
        }
        Instantiate(weapons[id - 1]);
        audioSource.PlayOneShot(weaponPickupSound);
    }

    IEnumerator TitleScreenTransition()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            TakeDamage(10);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (collision.gameObject.tag == "WeaponPickup")
            {
                int id = collision.gameObject.GetComponent<PickupWeapon>().weaponID;
                EquipWeapon(id);
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == "House")
            {
                level++;
                SceneManager.LoadScene(level);
            }
        }
    }
}
