using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : MonoBehaviour
{
    [SerializeField] float _hitMargin = 1.5f;
    [SerializeField] float _travelSpeed = 7.5f;
    
    Player player;
    float _innerRadius;
    float _outerRadius;
    int _damage;
    bool _hitPlayer = false;

    float timer = 0f;

    public Player Target
    {
        get => player;
        set => player = value;
    }

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    void Awake()
    {
        _innerRadius = 0;
        _outerRadius = _hitMargin;

        Transform epicenter = transform.GetChild(0);
        epicenter.position = new Vector3(transform.position.x, transform.position.y + 0.216f, transform.position.z - 0.125f);
        
        ParticleSystem epicenterParticles = epicenter.GetComponent<ParticleSystem>();
        ParticleSystem diffuseParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        Transform walls = GameObject.Find("/Board/GameWalls").GetComponent<Transform>();

        for (int i = 0; i < walls.childCount; i++)
        {
            epicenterParticles.trigger.AddCollider(walls.GetChild(i).GetComponent<Collider>());
            diffuseParticles.trigger.AddCollider(walls.GetChild(i).GetComponent<Collider>());
        }
    }

    void FixedUpdate()
    {
        _innerRadius += _travelSpeed * Time.deltaTime;
        _outerRadius += _travelSpeed * Time.deltaTime;

        bool playerInAreaXZ = Vector3.Distance(transform.position, player.transform.position) >= _innerRadius && Vector3.Distance(transform.position, player.transform.position) <= _outerRadius;
        bool playerInAreaY = transform.position.y > player.transform.position.y;
        if (playerInAreaXZ && playerInAreaY && !_hitPlayer)
        {
            player.DecreaseHealth(_damage);
            _hitPlayer = true;
        }

        timer += Time.deltaTime;
        if (timer >= 3)
        {
            Destroy(this.gameObject);
        }
    }
}
