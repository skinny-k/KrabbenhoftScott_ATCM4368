using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] float _duration = 5f;
    
    protected abstract void PowerUp(TankPlayer player);
    protected abstract void PowerDown(TankPlayer player);

    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;
    [SerializeField] float _movementSpeed = 1f;
    protected float MovementSpeed => _movementSpeed;

    protected Rigidbody _rb;
    protected Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
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
        TankPlayer player = other.gameObject.GetComponent<TankPlayer>();
        if (player != null)
        {
            TankController controller = player.GetComponent<TankController>();
            PowerUp(player);
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            Feedback();
            StartCoroutine(Cooldown(player));
        }
    }

    private void Feedback()
    {
        if (_collectParticles != null)
        {
            // _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
            // This results in a bunch of floating unused particles and causes issues with the Destroy
            // stop action. The line below works with Destroy stop action to clean up after itself.
            Instantiate(_collectParticles, transform.position, Quaternion.identity);
        }
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }

    private IEnumerator Cooldown(TankPlayer player)
    {
        yield return new WaitForSeconds(_duration);
        PowerDown(player);
        Destroy(gameObject);
    }
}
