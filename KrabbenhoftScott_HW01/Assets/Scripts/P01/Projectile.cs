using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected abstract void Impact(Collision otherCollision);

    [Header("Base Settings")]
    [SerializeField] protected float TravelSpeed = .25f;
    [SerializeField] protected ParticleSystem _impactParticles;
    [SerializeField] protected AudioClip _impactSFX;
    protected Rigidbody RB;

    void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Feedback();
        Impact(collision);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector3 moveOffset = transform.forward * TravelSpeed;
        RB.MovePosition(RB.position + moveOffset);
    }
    protected virtual void Feedback()
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

