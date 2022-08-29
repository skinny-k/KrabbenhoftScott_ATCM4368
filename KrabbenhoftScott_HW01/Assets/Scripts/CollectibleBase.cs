using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour
{
    protected abstract void Collect(Player player);

    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;
    [SerializeField] float _movementSpeed = 1f;
    protected float MovementSpeed => _movementSpeed;

    protected Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            TankController controller = player.GetComponent<TankController>();
            Collect(player);
            Feedback();
            gameObject.SetActive(false);
        }
    }

    private void Feedback()
    {
        if (_collectParticles != null)
        {
            _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
        }
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }
}
