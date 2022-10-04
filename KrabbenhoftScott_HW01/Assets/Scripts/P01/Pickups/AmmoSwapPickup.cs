using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSwapPickup : Collectible
{
    [Header("Ammo Pickup Settings")]
    [SerializeField] Projectile _ammoPrefab;
    [SerializeField] int _ammoCount;

    public static event Action<Projectile, int> OnCollect;
    
    protected override void Collect(Player player)
    {
        OnCollect?.Invoke(_ammoPrefab, _ammoCount);
    }
}
