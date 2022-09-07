using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Bullet _bullet;
    [SerializeField] protected ParticleSystem _fireParticles;
    [SerializeField] protected AudioClip _fireSFX;
    [SerializeField] float fireRate = 0.25f;

    float cooldown = 0;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && cooldown >= fireRate)
        {
            if (_fireParticles != null)
            {
                Instantiate(_fireParticles, transform.position, transform.rotation);
            }
            if (_fireSFX != null)
            {
                AudioHelper.PlayClip2D(_fireSFX, 0.5f);
            }
            Fire();
            cooldown = 0;
        }

        cooldown += Time.deltaTime;
    }

    void Fire()
    {
        if (_bullet != null)
        {
            Instantiate(_bullet, transform.position, transform.rotation);
        }
    }
}
