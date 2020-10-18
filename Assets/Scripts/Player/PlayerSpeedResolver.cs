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
        playerSpeedProcessed = playerInput.RunInput && playerMovements.CanRun ? playerStats.PlayerSpeedWhileRunning : playerSpeedProcessed;
        playerSpeedProcessed = playerInput.CrouchInput && playerMovements.CanCrouch ? playerSpeedProcessed / 2 : playerSpeedProcessed;

        if (playerMovements.IsMoving)
        {
            if (playerInput.RunInput)
                playerSpeedProcessed = playerRoll.IsRolling ? playerStats.PlayerSpeedWhileRunRolling : playerSpeedProcessed;
            else
                playerSpeedProcessed = playerRoll.IsRolling ? playerStats.PlayerSpeedWhileWalkRolling : playerSpeedProcessed;
        }
        else
            playerSpeedProcessed = playerRoll.IsRolling ? playerStats.PlayerSpeedWhileIdleRolling : playerSpeedProcessed;

        if (playerAttackPRocessor.IsLightAttacking)
            playerSpeedProcessed = playerStats.PlayerSpeedLightAttacking;
        if (playerAttackPRocessor.IsHeavyAttacking)
            playerSpeedProcessed = playerStats.PlayerSpeedHeavyAttacking;

        if(PlayerAbilityProcessor.Instance.IsExecutingAbility && PlayerAbilityProcessor.Instance.AbilityBeingExecuted.PlayerSpeedWhileUsingAbility != null)
            playerSpeedProcessed = (float) PlayerAbilityProcessor.Instance.AbilityBeingExecuted.PlayerSpeedWhileUsingAbility;

        //if (doingHurricane)
        //    playerSpeedProcessed = playerSpeedDoingHurricane;

        playerStats.CurrentSpeed = playerSpeedProcessed;
        return playerSpeedProcessed;
    }
}
