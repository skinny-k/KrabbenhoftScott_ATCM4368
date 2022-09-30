using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : Collectible
{
    [Header("Damage Boost Settings")]
    [SerializeField] int _boostPercent = 50;
    [SerializeField] float _duration = 5f;
    
    protected override void Collect(Player player)
    {
        player.ModifyDamage(1 + (_boostPercent / 100f), _duration);
    }
}
