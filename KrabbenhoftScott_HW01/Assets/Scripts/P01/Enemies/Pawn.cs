using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy
{
    [SerializeField] ParticleSystem _jumpParticles;
    [SerializeField] float _jumpPower = 10f;
    public Player player;

    Rigidbody _rb;
    float _moveModifier = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Game Plane")
        {
            Instantiate(_jumpParticles, transform.position, transform.rotation);
        }
    }
}
