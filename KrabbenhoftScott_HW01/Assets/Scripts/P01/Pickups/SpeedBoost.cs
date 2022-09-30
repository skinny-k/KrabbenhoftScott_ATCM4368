using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Collectible
{
    [Header("Speed Boost Settings")]
    [SerializeField] int _boostPercent = 50;
    [SerializeField] float _duration = 5f;
    
    protected override void Collect(Player player)
    {
        player.ModifySpeed(1 + (_boostPercent / 100f), _duration);
    }
}
