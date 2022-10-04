using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Player _player;
    
    void Update()
    {
        if (_player != null)
        {
            transform.position = _player.transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _player = other.gameObject.GetComponent<Player>();
        }
    }
}
