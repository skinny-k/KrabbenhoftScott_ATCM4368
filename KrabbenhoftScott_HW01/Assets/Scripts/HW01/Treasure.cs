using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : CollectibleBase
{
    [SerializeField] int _value = 1;

    protected override void Collect(TankPlayer player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.Treasure += _value;
            Debug.Log("Player's treasure: " + inventory.Treasure);
        }
    }

    protected override void Movement(Rigidbody rb)
    {
        Quaternion turnOffset = Quaternion.Euler(0, MovementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }
}
