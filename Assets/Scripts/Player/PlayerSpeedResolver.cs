using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedResolver : IPlayerSpeedResolver
{
    public float playerSpeedProcessed { get; private set; }

    public float SpeedResolver(
        PlayerStats playerStats, 
        IPlayerInput playerInput, 
        IPlayerMovements playerMovements, 
        IPlayerRoll playerRoll, 
        IPlayerAttacksProcessor playerAttackPRocessor)
    {
        playerSpeedProcessed = playerStats.InitialSpeed;

        playerSpeedProcessed = playerMovements.IsRunning && playerMovements.CanRun ? playerStats.PlayerSpeedWhileRunning : playerSpeedProcessed;
        playerSpeedProcessed = playerMovements.IsCrouching && playerMovements.CanCrouch ? playerSpeedProcessed / 2 : playerSpeedProcessed;

        if (playerAttackPRocessor.IsLightAttacking)
            playerSpeedProcessed = playerStats.PlayerSpeedLightAttacking;
        if (playerAttackPRocessor.IsHeavyAttacking)
            playerSpeedProcessed = playerStats.PlayerSpeedHeavyAttacking;

        if (playerRoll.IsRolling)
        {
            if (playerMovements.IsMoving)
            {
                if (playerMovements.IsRunning)
                    playerSpeedProcessed = playerStats.PlayerSpeedWhileRunRolling;
                else
                    playerSpeedProcessed = playerStats.PlayerSpeedWhileWalkRolling;
            }
        }

        if (PlayerAbilityProcessor.Instance.IsExecutingAbility && PlayerAbilityProcessor.Instance.AbilityBeingExecuted.PlayerSpeedWhileUsingAbility != null)
            playerSpeedProcessed = (float) PlayerAbilityProcessor.Instance.AbilityBeingExecuted.PlayerSpeedWhileUsingAbility;

        playerStats.CurrentSpeed = playerSpeedProcessed;
        return playerSpeedProcessed;
    }
}
