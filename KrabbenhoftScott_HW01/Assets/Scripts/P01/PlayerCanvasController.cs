using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] float _damageFlashDuration = 0.25f;
    [SerializeField] Player _player;
    
    Health _playerHealth;
    Image _HUD_damaged;
    float _damageTime = 0f;
    
    void Awake()
    {
        _HUD_damaged = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    void OnEnable()
    {
        _playerHealth = _player.GetComponent<Health>();
        _playerHealth.OnTakeDamage += FlashDamage;
    }
    
    void Update()
    {
        if (_HUD_damaged.gameObject.activeSelf)
        {
            if (_damageTime <= _damageFlashDuration)
            {
                float alpha = (_damageFlashDuration - _damageTime) / _damageFlashDuration;
                Color _tintColor = new Color(1, 1, 1, alpha);
                _HUD_damaged.color = _tintColor;
            }
            else
            {
                _HUD_damaged.color = Color.white;
                _HUD_damaged.gameObject.SetActive(false);
                _damageTime = 0;
            }
            _damageTime += Time.deltaTime;
        }
    }
    
    void FlashDamage()
    {
        _HUD_damaged.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        _playerHealth.OnTakeDamage -= FlashDamage;
    }
}
