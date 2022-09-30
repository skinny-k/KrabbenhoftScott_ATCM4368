using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : Projectile
{
    [SerializeField] int _damage = 10;

    float _damageModifier = PlayerGun._damageModifier;
    
    protected override void Impact(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.DecreaseHealth(transform, (int)(_damage * _damageModifier));
        }
        Destroy(gameObject);
    }

    protected override void Feedback(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            base.Feedback();
        }
    }
}
