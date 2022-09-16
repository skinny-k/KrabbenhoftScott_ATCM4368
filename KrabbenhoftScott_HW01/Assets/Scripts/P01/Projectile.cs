using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Projectile : MonoBehaviour
{
    protected abstract void Impact(Collision otherCollision);

    [Header("Base Settings")]
    [SerializeField] protected float _travelSpeed = .25f;
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected ParticleSystem _impactParticles;
    [SerializeField] float _SFXVolume = 0.75f;

    protected Rigidbody _rb;
    protected AudioClip _ricochetSFX;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        SFXContainer ricochetSFXContainer = GameObject.Find("RicochetSFXContainer").GetComponent<SFXContainer>();
        _ricochetSFX = ricochetSFXContainer.SFX[Random.Range(0, ricochetSFXContainer.Length)];
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
        if (_ricochetSFX != null)
        {
            AudioHelper.PlayClip3D(_ricochetSFX, _SFXVolume, transform.position);
        }
    }

    protected virtual void Feedback(Collision collision)
    {
        if (_impactParticles != null)
        {
            Instantiate(_impactParticles, transform.position, transform.rotation);
        }
        if (_ricochetSFX != null)
        {
            AudioHelper.PlayClip3D(_ricochetSFX, _SFXVolume, transform.position);
        }
    }
}

