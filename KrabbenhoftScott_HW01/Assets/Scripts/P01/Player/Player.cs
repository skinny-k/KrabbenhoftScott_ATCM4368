using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Health _health;
    PlayerMovement _movement;
    PlayerGun _gun;

    public Health MyHealth
    {
        get => _health;
    }
    public PlayerMovement MyMovement
    {
        get => _movement;
    }
    public PlayerGun MyGun
    {
        get => _gun;
    }

    void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<PlayerMovement>();
        _gun = transform.GetChild(4).GetComponent<PlayerGun>();
    }

    public void ModifyDamage(float damageModifier, float duration)
    {
        StartCoroutine(_gun.ModifyDamage(damageModifier, duration));
    }

    public void ModifySpeed(float moveModifier, float duration)
    {
        StartCoroutine(_movement.ModifySpeed(moveModifier, duration));
    }

    public void ModifyDefense(float defenseModifier, float duration)
    {
        StartCoroutine(_health.ModifyDefense(defenseModifier, duration));
    }

    public void GiveAmmo(Projectile ammo, int count)
    {
        _gun.GiveAmmo(ammo, count);
    }

    public void DecreaseHealth(Transform source, int damage)
    {
        _health.DecreaseHealth(source, damage);
    }
    
    public void DecreaseHealth(int damage)
    {
        _health.DecreaseHealth(damage);
    }
    
    public void IncreaseHealth(int heal)
    {
        _health.IncreaseHealth(heal);
    }
}
