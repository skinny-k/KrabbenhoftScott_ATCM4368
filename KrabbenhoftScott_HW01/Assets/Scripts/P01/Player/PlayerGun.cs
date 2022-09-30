using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Projectile _projectile;
    [SerializeField] protected ParticleSystem _fireParticles;
    [SerializeField] protected AudioClip _fireSFX;
    [SerializeField] float fireRate = 0.25f;

    public static float _damageModifier = 1f;
    float cooldown = 0;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldown >= fireRate)
        {
            if (_fireParticles != null)
            {
                Instantiate(_fireParticles, transform.position, transform.rotation);
            }
            if (_fireSFX != null)
            {
                AudioHelper.PlayClip2D(_fireSFX, 0.1f);
            }
            Fire();
            cooldown = 0;
        }

        cooldown += Time.deltaTime;
    }

    void Fire()
    {
        if (_projectile != null)
        {
            Instantiate(_projectile, transform.position, transform.rotation);
        }
    }

    public IEnumerator ModifyDamage(float damageModifier, float duration)
    {
        _damageModifier *= damageModifier;

        yield return new WaitForSeconds(duration);

        _damageModifier /= damageModifier;
    }
}
