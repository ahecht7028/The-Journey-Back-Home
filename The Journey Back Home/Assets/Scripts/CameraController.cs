using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isDead = false;
    Vector3 mousePos;
    Vector3 playerPos;
    //AudioSource audioSource;
    //public AudioClip music;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        playerPos = GameObject.Find("Player").transform.position;
        if (!isDead)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 halfPoint = new Vector3((playerPos.x + mousePos.x) / 2, (playerPos.y + mousePos.y) / 2, -10);
            transform.position = new Vector3((playerPos.x + halfPoint.x) / 2, (playerPos.y + halfPoint.y) / 2, -10);
        }
        else
        {
            transform.position = playerPos - new Vector3(0, 0, 10);
        }
    }
}
