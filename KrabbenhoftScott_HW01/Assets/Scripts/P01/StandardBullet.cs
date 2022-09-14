using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : Projectile
{
    protected override void Impact(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.DecreaseHealth(transform, _damage);
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
