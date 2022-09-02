using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerUp : PowerUpBase
{
    protected override void PowerUp(TankPlayer player)
    {
        player.Invincible = true;
        Debug.Log("Player is invincible!");
    }

    protected override void PowerDown(TankPlayer player)
    {
        player.Invincible = false;
        Debug.Log("Invicibility duration ended.");
    }
}
