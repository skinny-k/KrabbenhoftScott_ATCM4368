using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerBullet : Projectile
{
    [SerializeField] float _lifetime = 5f;
    [SerializeField] float _turnSpd = 5f;

    GameObject boss;
    Quaternion _lookRot;
    float _timer = 0f;
    
    void Awake()
    {
        boss = GameObject.Find("Boss");
    }
    
    void FixedUpdate()
    {
        if (boss != null)
        {
            _lookRot = Quaternion.LookRotation((boss.transform.position - transform.position).normalized);
            _rb.rotation = Quaternion.Lerp(_rb.rotation, _lookRot, _turnSpd * Time.deltaTime);
        }
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
