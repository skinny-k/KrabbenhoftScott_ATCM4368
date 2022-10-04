using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    protected abstract void Collect(Player player);

    [SerializeField] float _rotationSpeed = 90f;
    [SerializeField] float _bobPeriod = 1f;
    [SerializeField] float _bobAmplitude = 0.25f;
    [SerializeField] float _lifetime = 5f;

    [Header("Feedback Settings")]
    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSFX;
    [SerializeField] float _collectSFXVolume = 0.75f;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Despawn());
    }
    
    void FixedUpdate()
    {
        Spin();
        Bob();
    }
    
    void Spin()
    {
        Quaternion turnOffset = Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    void Bob()
    {
        _rb.MovePosition(new Vector3(transform.position.x, _bobAmplitude * Mathf.Sin(((2 * Mathf.PI) / _bobPeriod) * Time.time), transform.position.z));
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Collect(player);
            Feedback(other.transform.position);
            Destroy(gameObject);
        }
    }

    protected virtual void Feedback(Vector3 location)
    {
        if (_collectParticles != null)
        {
            ParticleSystem spawnedParticles = Instantiate(_collectParticles, location, Quaternion.identity);
        }
        if (_collectSFX != null)
        {
            AudioHelper.PlayClip2D(_collectSFX, _collectSFXVolume);
        }
    }

    protected virtual IEnumerator Despawn()
    {
        yield return new WaitForSeconds(_lifetime - 1.2f);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(!transform.GetChild(j).gameObject.activeSelf);
            }
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }
}
