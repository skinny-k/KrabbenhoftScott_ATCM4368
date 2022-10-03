using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Projectile : MonoBehaviour
{
    protected abstract void Impact(Collision otherCollision);

    [Header("Base Settings")]
    [SerializeField] protected float _travelSpeed = .25f;
    [SerializeField] protected ParticleSystem _impactParticles;

    protected Rigidbody _rb;

    public static event Action<Projectile> OnImpact;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Feedback(collision);
        Impact(collision);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector3 moveOffset = transform.forward * _travelSpeed;
        _rb.MovePosition(_rb.position + moveOffset);
    }

    protected virtual void Feedback()
    {
        if (_impactParticles != null)
        {
            Instantiate(_impactParticles, transform.position, transform.rotation);
        }
        OnImpact?.Invoke(this);
    }

    protected virtual void Feedback(Collision collision)
    {
        if (_impactParticles != null)
        {
            Instantiate(_impactParticles, transform.position, transform.rotation);
        }
        OnImpact?.Invoke(this);
    }
}

