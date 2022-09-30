using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXContainer : MonoBehaviour
{
    [SerializeField] float _SFXVolume = 0.75f;
    [SerializeField] AudioClip[] SFX;

    void OnEnable()
    {
        Projectile.OnImpact += PlayImpactSFX;
    }

    void PlayImpactSFX(Transform impactLocation)
    {
        AudioHelper.PlayClip3D(SFX[UnityEngine.Random.Range(0, SFX.Length)], _SFXVolume, impactLocation.position);
    }

    void OnDisable()
    {
        Projectile.OnImpact -= PlayImpactSFX;
    }
}
