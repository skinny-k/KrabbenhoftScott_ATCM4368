using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    protected override void Impact(Collision collision)
    {
        Health target = collision.gameObject.GetComponent<Health>();
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
            base.Feedback(collision);
        }
    }
}
