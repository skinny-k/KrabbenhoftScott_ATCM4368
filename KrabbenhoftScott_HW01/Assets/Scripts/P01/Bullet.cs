using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    protected override void Impact(Collision collision)
    {
        Destroy(gameObject);
    }
}
