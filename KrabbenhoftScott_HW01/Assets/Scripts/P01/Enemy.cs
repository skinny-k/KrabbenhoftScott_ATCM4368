using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected abstract void Move();

    [SerializeField] protected float _moveSpeed = 5f;

    void FixedUpdate()
    {
        Move();
    }
}
