using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Player _player;
    
    Health _playerHealth;
    Vector3 _startPos;
    bool _shaking = false;

    void Awake()
    {
        _startPos = transform.position;
    }

    void OnEnable()
    {
        _playerHealth = _player.GetComponent<Health>();
        _playerHealth.OnTakeDamage += ShakeAction;
    }
    
    void FixedUpdate()
    {
        if (_shaking)
        {
            transform.position = _startPos + new Vector3(Random.Range(-50, 50) / 100f, Random.Range(-50, 50) / 100f, Random.Range(-50, 50) / 100f);
        }
    }
    
    public IEnumerator Shake(float _shakeDuration = 0.25f)
    {
        _shaking = true;
        
        yield return new WaitForSeconds(_shakeDuration);

        _shaking = false;
        transform.position = _startPos;
    }

    public void ShakeAction()
    {
        StartCoroutine(Shake(0.1f));
    }

    void OnDisable()
    {
        _playerHealth.OnTakeDamage -= ShakeAction;
    }
}
