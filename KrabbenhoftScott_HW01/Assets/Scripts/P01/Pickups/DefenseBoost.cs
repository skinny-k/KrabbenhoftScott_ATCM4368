using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoost : Collectible
{
    [Header("Defense Boost Settings")]
    [SerializeField] int _reductionPercent = 50;
    [SerializeField] float _duration = 5f;
    
    protected override void Collect(Player player)
    {
        player.ModifyDefense(_reductionPercent / 100f, _duration);
    }
}
