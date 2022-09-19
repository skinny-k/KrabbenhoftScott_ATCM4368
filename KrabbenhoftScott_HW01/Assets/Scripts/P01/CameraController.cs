using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 _startPos;
    bool _shaking = false;

    void Awake()
    {
        _startPos = transform.position;
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
}
