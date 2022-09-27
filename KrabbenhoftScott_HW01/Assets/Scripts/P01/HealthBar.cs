using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health _health;
    [SerializeField] float _easeDelay = 0.25f;
    [SerializeField] float _easeSpeed = 15f;
    
    TMP_Text _healthText;
    Slider _literalHealth;
    Slider _easedHealth;
    float _waitTime = 0f;

    void Awake()
    {
        _literalHealth = GetComponent<Slider>();
        _easedHealth = transform.GetChild(0).gameObject.GetComponent<Slider>();

        _literalHealth.maxValue = _health.MaxHealth;
        _literalHealth.value = _health.MaxHealth;
        _easedHealth.maxValue = _health.MaxHealth;
        _easedHealth.value = _health.MaxHealth;

        _healthText = transform.GetChild(2).GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        _health.OnSpawn += SetHealth;
        _health.OnTakeDamage += SetHealth;
    }

    void SetHealth()
    {
        _literalHealth.value = _health.CurrentHealth;
    }

    void Update()
    {
        if (_easedHealth.value != _health.CurrentHealth)
        {
            _waitTime += Time.deltaTime;
            if (_waitTime >= _easeDelay)
            {
                _easedHealth.value = Mathf.Clamp(_easedHealth.value - (Time.deltaTime * _easeSpeed), _health.CurrentHealth, _health.MaxHealth);
                if (_easedHealth.value == _health.CurrentHealth)
                {
                    _waitTime = 0f;
                }
            }
        }
        _healthText.text = _health.CurrentHealth + " / " + _health.MaxHealth;
    }
    
    void OnDisable()
    {
        _health.OnSpawn -= SetHealth;
        _health.OnTakeDamage -= SetHealth;
    }
}
