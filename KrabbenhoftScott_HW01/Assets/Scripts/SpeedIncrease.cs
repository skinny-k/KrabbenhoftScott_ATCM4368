using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : CollectibleBase
{
    [SerializeField] float _speedModifier = 2f;
    //[SerializeField] float _duration = 5f;

    protected override void Collect(Player player)
    {
        TankController controller = player.GetComponent<TankController>();
        if (controller != null)
        {
            //controller.MoveSpeed += _speedAdded;
            controller.MoveModifier *= _speedModifier;
            Debug.Log("Player's speed modifier: " + (controller.MoveModifier));
            //controller.StartCoroutine(controller.ResetSpeed(_speedModifier, _duration));
        }
    }

    protected override void Movement(Rigidbody rb)
    {
        Quaternion turnOffset = Quaternion.Euler(MovementSpeed, MovementSpeed, MovementSpeed);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }
}
