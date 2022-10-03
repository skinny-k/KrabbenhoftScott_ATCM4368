using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXContainer : MonoBehaviour
{
    [Header("Ricochet SFX")]
    [SerializeField] float _ricochetSFXVolume = 0.75f;
    [SerializeField] AudioClip[] _ricochetSFX;
    [Header("Explode SFX")]
    [SerializeField] float _explodeSFXVolume = 0.75f;
    [SerializeField] AudioClip[] _explodeSFX;
    [Header("Slam SFX")]
    [SerializeField] float _slamSFXVolume = 0.75f;
    [SerializeField] AudioClip[] _slamSFX;

    void OnEnable()
    {
        Projectile.OnImpact += PlayImpactSFX;
        Boss.OnSlam += PlaySlamSFX;
    }

    void PlayImpactSFX(Projectile projectile)
    {
        Vector3 impactPosition = projectile.transform.position;

        if (projectile is ExplosiveBullet)
        {
            AudioHelper.PlayClip3D(_explodeSFX[UnityEngine.Random.Range(0, _explodeSFX.Length)], _ricochetSFXVolume, impactPosition);
        }
        else
        {
            AudioHelper.PlayClip3D(_ricochetSFX[UnityEngine.Random.Range(0, _ricochetSFX.Length)], _explodeSFXVolume, impactPosition);
        }
    }

    void PlaySlamSFX()
    {
        AudioHelper.PlayClip2D(_slamSFX[UnityEngine.Random.Range(0, _slamSFX.Length)], _slamSFXVolume);
    }

    void OnDisable()
    {
        Projectile.OnImpact -= PlayImpactSFX;
        Boss.OnSlam -= PlaySlamSFX;
    }
}
