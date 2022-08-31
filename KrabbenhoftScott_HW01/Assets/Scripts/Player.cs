using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    bool _invincible = false;
    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }
    
    TankController _tankController;
    Inventory _inventory;

    UIHelper ui;

    Material m_body;
    Material m_tread_l;
    Material m_tread_r;
    Material m_turret;
    Color tankStartColor;
    Color treadStartColor;
    
    void Awake()
    {
        _tankController = GetComponent<TankController>();
        _inventory = GetComponent<Inventory>();

        ui = GameObject.Find("Canvas").GetComponent<UIHelper>();

        m_body    = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        m_tread_l = transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material;
        m_tread_r = transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material;
        m_turret  = transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material;

        tankStartColor  = m_body.color;
        treadStartColor = m_tread_l.color;
    }
    
    void Start()
    {
        _currentHealth = _maxHealth;
        ui.UpdateHealth(_currentHealth);
    }

    void Update()
    {
        if (_invincible)
        {
            Color tankColor = Color.HSVToRGB(Time.time % 1, 0.5f, 0.9f);
            m_body.color    = tankColor;
            m_turret.color  = tankColor;
            m_tread_l.color = Color.white;
            m_tread_r.color = Color.white;
        }
        else if (m_body.color != tankStartColor || m_tread_l.color != treadStartColor)
        {
            m_body.color    = tankStartColor;
            m_turret.color  = tankStartColor;
            m_tread_l.color = treadStartColor;
            m_tread_r.color = treadStartColor;
        }
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        ui.UpdateHealth(_currentHealth);
    }

    public void DecreaseHealth(int amount)
    {
        if (!_invincible)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            ui.UpdateHealth(_currentHealth);
            if (_currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        if (!_invincible)
        {
            _currentHealth = 0;
            ui.UpdateHealth(_currentHealth);
            gameObject.SetActive(false);
        }
    }
}
