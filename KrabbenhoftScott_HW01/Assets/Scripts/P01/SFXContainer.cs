using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXContainer : MonoBehaviour
{
    [SerializeField] public AudioClip[] SFX;

    public int Length
    {
        get => SFX.Length;
    }
}
