using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _lookTarget;
    [SerializeField] ParticleSystem _jumpParticles;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _jumpPower = 10f;
    Rigidbody _rb;
    float _moveModifier = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
        TurnPlayer();
    }

    void MovePlayer()
    {
        // calculate the move amount
        float moveX = Input.GetAxis("Horizontal") * _moveSpeed * _moveModifier * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical")   * _moveSpeed * _moveModifier * Time.deltaTime;
        // create a vector from amount and direction
        Vector3 moveOffset = new Vector3(moveX, 0, moveZ);
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
        if ((moveOffset != Vector3.zero) && Physics.OverlapSphere(transform.position, 0.01f, LayerMask.GetMask("Game Plane")).Length > 0)
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

    void TurnPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Game Plane"));
        if (hit.point != null)
        {
            _lookTarget.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(_lookTarget);
        }
    }
}
