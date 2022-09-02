using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] float _speedModifier = 0.3f;
    [SerializeField] float _duration = 1.5f;
    
    protected override void PlayerImpact(TankPlayer player)
    {
        TankController controller = player.GetComponent<TankController>();
        if (controller != null)
        {
            controller.MoveModifier *= _speedModifier;
            Debug.Log("Player's speed modifier: " + controller.MoveModifier);
            controller.StartCoroutine(controller.ResetSpeed(_speedModifier, _duration));
        }
    }
}
