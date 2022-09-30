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
    [SerializeField] float _shakeTime = 0.25f;
    
    RectTransform _shakeCanvas;
    TMP_Text _healthText;
    Slider _literalHealth;
    Slider _easedHealth;
    float _waitTime = 0f;
    bool _isShaking = false;

    void Awake()
    {
        _literalHealth = GetComponent<Slider>();
        _easedHealth = transform.GetChild(0).gameObject.GetComponent<Slider>();

        _literalHealth.maxValue = _health.MaxHealth;
        _literalHealth.value = _health.MaxHealth;
        _easedHealth.maxValue = _health.MaxHealth;
        _easedHealth.value = _health.MaxHealth;

        _shakeCanvas = transform.parent.GetComponent<RectTransform>();
        _healthText = transform.GetChild(2).GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        _health.OnSpawn += SetHealthBasic;
        _health.OnHeal += SetHealthBasic;
        _health.OnTakeDamage += SetHealth;
    }

    void SetHealthBasic()
    {
        _literalHealth.value = _health.CurrentHealth;
    }
    
    void SetHealth()
    {
        SetHealthBasic();
        StartCoroutine(Shake());
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
        if (_isShaking)
        {
            _shakeCanvas.anchoredPosition = new Vector2(Random.Range(-100, 101) / 10, Random.Range(-100, 101) / 10);
        }
        else if (_shakeCanvas.anchoredPosition != Vector2.zero)
        {
            _shakeCanvas.anchoredPosition = Vector2.zero;
        }
        _healthText.text = _health.CurrentHealth + " / " + _health.MaxHealth;
    }

    IEnumerator Shake()
    {
        _isShaking = true;

        yield return new WaitForSeconds(_shakeTime);

        _isShaking = false;
    }
    
    void OnDisable()
    {
        _health.OnSpawn -= SetHealthBasic;
        _health.OnTakeDamage -= SetHealth;
    }
}
