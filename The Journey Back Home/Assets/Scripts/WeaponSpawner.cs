using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public int rarity;
    public GameObject[] weaponpickups;

    void Start()
    {
        if(Random.Range(0, rarity) == 0)
        {
            Instantiate(weaponpickups[Random.Range(0, weaponpickups.Length)], transform.position, Quaternion.identity);
        }
    }
}
