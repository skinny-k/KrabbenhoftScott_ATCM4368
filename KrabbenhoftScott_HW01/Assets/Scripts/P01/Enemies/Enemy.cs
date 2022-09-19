using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected abstract void Move();

    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected int _contactDamage = 10;

    void FixedUpdate()
    {
        Move();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.GetComponent<Health>().DecreaseHealth(_contactDamage);
        }
    }
}
