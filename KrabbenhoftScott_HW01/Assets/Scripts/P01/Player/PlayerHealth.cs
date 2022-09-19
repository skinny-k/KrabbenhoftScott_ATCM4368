using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    CanvasController _HUD;

    public override void Awake()
    {
        _HUD = GetComponent<Player>()._HUD;
        base.Awake();
    }
    
    public override void DecreaseHealth(Transform source, int damage)
    {
        _HUD.FlashDamage();
        base.DecreaseHealth(source, damage);
    }
    
    public override void DecreaseHealth(int damage)
    {
        _HUD.FlashDamage();
        base.DecreaseHealth(damage);
    }
}
