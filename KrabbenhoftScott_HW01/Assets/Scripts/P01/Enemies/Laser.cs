using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float timer = 0f;
    int _damage = 0;

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    void Awake()
    {
        Transform beam = transform.GetChild(0);
        beam.position = new Vector3(transform.position.x, transform.position.y + 0.216f, transform.position.z - 0.125f);
        
        ParticleSystem beamParticles = beam.GetComponent<ParticleSystem>();
        ParticleSystem diffuseParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        Transform walls = GameObject.Find("/Board/GameWalls").GetComponent<Transform>();

        for (int i = 0; i < walls.childCount; i++)
        {
            beamParticles.trigger.AddCollider(walls.GetChild(i).GetComponent<Collider>());
            diffuseParticles.trigger.AddCollider(walls.GetChild(i).GetComponent<Collider>());
        }
    }
    
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.DecreaseHealth(_damage);
        }
    }
}
