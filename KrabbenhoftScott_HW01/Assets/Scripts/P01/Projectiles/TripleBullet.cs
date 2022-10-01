using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBullet : Projectile
{
    [SerializeField] int _damage = 10;
    [SerializeField] float _scatterAngle = 15f;
    [SerializeField] StandardBullet _standardBulletPrefab;

    float _damageModifier = PlayerGun._damageModifier;
    
    protected override void Awake()
    {
        Instantiate(_standardBulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, _scatterAngle, 0)));
        Instantiate(_standardBulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -_scatterAngle, 0)));

        base.Awake();
    }
    
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
