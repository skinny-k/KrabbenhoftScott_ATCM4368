using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] int _maxHealth = 30;
    [SerializeField] float _defenseModifier = 0f;
    [SerializeField] float _healModifier = 1f;

    [SerializeField] ParticleSystem _healParticles;
    [SerializeField] AudioClip _healSFX;
    [SerializeField] ParticleSystem _damageParticles;
    [SerializeField] AudioClip _damageSFX;
    [SerializeField] ParticleSystem _dieParticles;
    [SerializeField] AudioClip _dieSFX;

    int _currentHealth;

    public void Awake()
    {
        _currentHealth = _maxHealth;
    }
    
    public void DecreaseHealth(Transform source, int damage)
    {
        _currentHealth = (int)Mathf.Clamp(_currentHealth - (damage * (1f - _defenseModifier)), 0, _maxHealth);
        if (_currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            if (_damageParticles != null)
            {
                Instantiate(_damageParticles, source.position, source.rotation);
            }
            if (_damageSFX != null)
            {
                AudioHelper.PlayClip3D(_damageSFX, 1f, transform.position);
            }
        }
    }

    public void IncreaseHealth(int heal)
    {
        _currentHealth = (int)Mathf.Clamp(_currentHealth + (heal * _healModifier), 0, _maxHealth);
        if (_healParticles != null)
        {
            Instantiate(_healParticles, transform.position, transform.rotation);
        }
        if (_healSFX != null)
        {
            AudioHelper.PlayClip3D(_healSFX, 1f, transform.position);
        }
    }

    void Kill()
    {
        if (_dieParticles != null)
        {
            Instantiate(_dieParticles, transform.position, transform.rotation);
        }
        if (_dieSFX != null)
        {
            AudioHelper.PlayClip3D(_dieSFX, 1f, transform.position);
        }
        gameObject.SetActive(false);
    }
}
