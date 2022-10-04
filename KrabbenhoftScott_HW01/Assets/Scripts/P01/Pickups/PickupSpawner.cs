using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] Collectible[] pickups;

    public static PickupSpawner Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SpawnPickup(Vector3 location)
    {
        Instantiate(Instance.pickups[UnityEngine.Random.Range(0, Instance.pickups.Length)], location, Quaternion.identity);
    }
}
