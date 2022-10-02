using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy, IDropsPickups
{
    [SerializeField] ParticleSystem _jumpParticles;
    [SerializeField] float _jumpPower = 10f;
    [SerializeField] int _pickupDropChance = 60;
    public Player player;

    Rigidbody _rb;
    Health _health;
    float _moveModifier = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
    }

    void OnEnable()
    {
        _health.OnDie += SpawnPickups;
    }
    
    protected override void Move()
    {
        Vector3 moveOffset = player.transform.position - _rb.position;
        _rb.MovePosition(_rb.position + moveOffset * _moveSpeed / Vector3.Distance(transform.position, player.transform.position) * _moveModifier * Time.deltaTime);
        
        if (moveOffset != Vector3.zero && Physics.OverlapSphere(transform.position, 0.01f, LayerMask.GetMask("Game Plane")).Length > 0)
        {
            _rb.AddForce(new Vector3(0, _jumpPower, 0));
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Game Plane")
        {
            Instantiate(_jumpParticles, transform.position, transform.rotation);
        }
        else
        {
            base.OnCollisionEnter(collision);
        }
    }

    public void SpawnPickups()
    {
        if (Random.Range(0, 101) <= _pickupDropChance)
        {
            PickupSpawner.SpawnPickup(transform.position);
        }
    }

    void OnDisable()
    {
        _health.OnDie -= SpawnPickups;
        Destroy(gameObject);
    }
}
