using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerUp : PowerUpBase
{
    protected override void PowerUp(Player player)
    {
        player.Invincible = true;
        Debug.Log("Player is invincible!");
    }

    protected override void PowerDown(Player player)
    {
        player.Invincible = false;
        Debug.Log("Invicibility duration ended.");
    }
}
