using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Projectile
{
    [SerializeField] int _impactDamage = 10;
    [SerializeField] int _explosionDamage = 20;
    [SerializeField] float _explosionRadius = 2f;
    [SerializeField] AudioClip _explosionSFX;

    float _damageModifier = PlayerGun._damageModifier;
    
    protected override void Impact(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.DecreaseHealth(transform, (int)(_impactDamage * _damageModifier));
        }
        Explode();
        Destroy(gameObject);
    }

    protected override void Feedback(Collision collision)
    {
        if (_impactParticles != null)
        {
            Instantiate(_impactParticles, transform.position, transform.rotation);
        }
        if (_explosionSFX != null)
        {
            AudioHelper.PlayClip2D(_explosionSFX, _SFXVolume);
        }
    }

    void Explode()
    {
        Object[] enemies = Object.FindObjectsOfType(typeof(Enemy));

        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<Health>().DecreaseHealth((int)(_explosionDamage * _damageModifier));
        }
    }
}
