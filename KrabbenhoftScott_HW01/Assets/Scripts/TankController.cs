using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    [SerializeField] float _turnSpeed = 2f;
    /*
    [SerializeField] float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
    }
    */
    [SerializeField] float _moveModifier = 1f;
    public float MoveModifier
    {
        get => _moveModifier;
        set => _moveModifier = value;
    }

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //_moveSpeed = _baseSpeed;
        Debug.Log("Awake! Player's speed: " + _moveSpeed);
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed * _moveModifier;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    /*
    public IEnumerator ResetSpeed(float modifier, float duration)   
    {
        yield return new WaitForSeconds(duration);
        MoveSpeed = _baseSpeed;
        Debug.Log("Speed reset! Player's speed: " + _MoveSpeed);
    }
    */
    
    public IEnumerator ResetSpeed(float modifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        MoveModifier /= modifier;
        Debug.Log("Speed reset! Player's speed: " + _moveModifier);
    }
}
