using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Projectile _standardProjectile;
    [SerializeField] protected ParticleSystem _fireParticles;
    [SerializeField] protected AudioClip _fireSFX;
    [SerializeField] float fireRate = 0.25f;

    public event Action<Projectile, int> OnAmmoChange;
    
    Projectile _projectile;
    int _specialAmmoCount = 0;
    
    public static float _damageModifier = 1f;
    float cooldown = 0;
    
    void OnEnable()
    {
        _projectile = _standardProjectile;
        _specialAmmoCount = 0;
        
        AmmoSwapPickup.OnCollect += GiveAmmo;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldown >= fireRate)
        {
            if (_fireParticles != null)
            {
                Instantiate(_fireParticles, transform.position, transform.rotation);
            }
            if (_fireSFX != null)
            {
                AudioHelper.PlayClip2D(_fireSFX, 0.1f);
            }
            Fire();
            cooldown = 0;
        }

        cooldown += Time.deltaTime;
    }

    void Fire()
    {
        if (_projectile != null)
        {
            Instantiate(_projectile, transform.position, transform.rotation);
            if (_projectile != _standardProjectile)
            {
                _specialAmmoCount--;
                if (_specialAmmoCount <= 0)
                {
                    _projectile = _standardProjectile;
                }
            }
            OnAmmoChange?.Invoke(_projectile, _specialAmmoCount);
        }
    }

    public void GiveAmmo(Projectile ammo, int count)
    {
        _projectile = ammo;
        _specialAmmoCount = count;
        OnAmmoChange?.Invoke(_projectile, _specialAmmoCount);
    }

    public IEnumerator ModifyDamage(float damageModifier, float duration)
    {
        _damageModifier *= damageModifier;

        yield return new WaitForSeconds(duration);

        _damageModifier /= damageModifier;
    }

    void OnDisable()
    {
        AmmoSwapPickup.OnCollect -= GiveAmmo;
    }
}
