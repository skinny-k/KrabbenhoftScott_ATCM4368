using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected abstract void Impact(Collision otherCollision);

    [Header("Base Settings")]
    [SerializeField] protected float _travelSpeed = .25f;
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected ParticleSystem _impactParticles;
    [SerializeField] protected AudioClip _impactSFX;
    protected Rigidbody RB;

    void Awake()
    {
        RB = GetComponent<Rigidbody>();
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
        RB.MovePosition(RB.position + moveOffset);
    }
    protected virtual void Feedback(Collision collision)
    {
        if (_impactParticles != null)
        {
            Instantiate(_impactParticles, transform.position, transform.rotation);
        }
        if (_impactSFX != null)
        {
            AudioHelper.PlayClip3D(_impactSFX, 1.0f, transform.position);
        }
    }
}

