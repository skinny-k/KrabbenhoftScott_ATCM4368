using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    ParticleSystem dieParticles = null; // the actual instance of particles spawned on death, NOT THE PREFAB OBJECT
    public bool isDying = false;
    public float dyingFor = 0;
    
    void FixedUpdate()
    {
        if (isDying && dieParticles != null && dyingFor <= 3f)
        {
            transform.localScale = new Vector3(2, 1.25f + Random.Range(0, 75) / 100f, 2);
            dyingFor += Time.deltaTime;
        }
        else if (dyingFor > 3f)
        {
            dyingFor += Time.deltaTime;
            transform.localScale = new Vector3(2 - (4 * (dyingFor - 3f)), 2 - (4 * (dyingFor - 3f)), 2 - (4 * (dyingFor - 3f)));
            if (dyingFor >= 3.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    
    protected override void Kill()
    {
        if (_dieParticles != null)
        {
            dieParticles = Instantiate(_dieParticles, transform.position, transform.rotation);
        }
        if (_dieSFX != null)
        {
            AudioHelper.PlayClip3D(_dieSFX, 1f, transform.position);
        }

        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(Camera.main.GetComponent<CameraController>().Shake(3.25f));
        isDying = true;
    }
}
