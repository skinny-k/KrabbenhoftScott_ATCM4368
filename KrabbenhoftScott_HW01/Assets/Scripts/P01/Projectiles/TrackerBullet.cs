using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerBullet : Projectile
{
    [SerializeField] int _damage = 10;
    [SerializeField] float _lifetime = 5f;
    [SerializeField] float _turnSpd = 5f;

    Enemy target;
    Quaternion _lookRot;
    float _timer = 0f;

    float _damageModifier = PlayerGun._damageModifier;
    
    void Awake()
    {
        target = FindTarget();
        base.Awake();
    }
    
    protected override void Move()
    {
        if (target != null)
        {
            _lookRot = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
            _rb.rotation = Quaternion.Lerp(_rb.rotation, _lookRot, _turnSpd * Time.deltaTime);
        }
        base.Move();

        _timer += Time.deltaTime;
        if (_timer >= _lifetime)
        {
            base.Feedback();
        }
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

    Enemy FindTarget()
    {
        Object[] enemies = Object.FindObjectsOfType(typeof(Enemy));
        
        Enemy nearestEnemy = null;
        
        if (enemies.Length > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                if (nearestEnemy == null || Vector3.Distance(transform.position, nearestEnemy.transform.position) < Vector3.Distance(transform.position, enemy.transform.position))
                {
                    nearestEnemy = enemy;
                }
            }
            return nearestEnemy;
        }
        else
        {
            return null;
        }
    }
}
