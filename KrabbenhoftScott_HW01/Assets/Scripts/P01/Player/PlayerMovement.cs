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
    bool _inAir = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_inAir)
        {
            _rb.AddForce(new Vector3(0, _jumpPower * 2.5f, 0));
            _inAir = true;
        }
    }
    
    void FixedUpdate()
    {
        MovePlayer();
        TurnPlayer();
    }

    void MovePlayer()
    {
        // calculate the move amount
        Vector3 moveOffset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset * _moveSpeed * _moveModifier * Time.deltaTime);
        // technically adjusting vector is more accurate! (but more complex)
        if ((moveOffset != Vector3.zero) && Physics.OverlapSphere(transform.position, 0.01f, LayerMask.GetMask("Game Plane")).Length > 0)
        {
            _rb.AddForce(new Vector3(0, _jumpPower, 0));
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Game Plane")
        {
            Instantiate(_jumpParticles, transform.position, transform.rotation);
            _inAir = false;
        }
    }

    public IEnumerator ModifySpeed(float moveModifier, float duration)
    {
        _moveModifier *= moveModifier;

        yield return new WaitForSeconds(duration);

        _moveModifier /= moveModifier;
    }
}
