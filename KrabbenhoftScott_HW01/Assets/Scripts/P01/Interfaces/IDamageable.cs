using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void DecreaseHealth(Transform source, int damage);
}
