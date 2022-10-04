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

    void Explode()
    {
        Object[] enemies = Object.FindObjectsOfType(typeof(Enemy));

        foreach (Enemy enemy in enemies)
        {
            Debug.Log(Vector3.Distance(transform.position, enemy.transform.position));
            if (Vector3.Distance(transform.position, enemy.transform.position) < _explosionRadius)
            {
                enemy.GetComponent<Health>().DecreaseHealth((int)(_explosionDamage * _damageModifier));
            }
        }
    }
}
