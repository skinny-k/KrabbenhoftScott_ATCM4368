using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected abstract void Impact(Collision otherCollision);

    [Header("Base Settings")]
    [SerializeField] protected float TravelSpeed = .25f;
    [SerializeField] protected ParticleSystem _fireParticles;
    [SerializeField] protected AudioClip _fireSFX;
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
        //
    }
}

