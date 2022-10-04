using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Collectible
{
    [SerializeField] int _healAmount = 5;
    
    protected override void Collect(Player player)
    {
        player.IncreaseHealth(_healAmount);
    }
}
