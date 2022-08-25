using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : CollectibleBase
{
    [SerializeField] float _speedAdded = 5;

    protected override void Collect(Player player)
    {
        TankController controller = player.GetComponent<TankController>();
        if (controller != null)
        {
            controller.MaxSpeed += _speedAdded;
            Debug.Log("Player's speed: " + controller.MaxSpeed);
        }
    }

    protected override void Movement(Rigidbody rb)
    {
        Quaternion turnOffset = Quaternion.Euler(MovementSpeed, MovementSpeed, MovementSpeed);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }
}
