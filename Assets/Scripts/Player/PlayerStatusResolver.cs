using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusResolver : IPlayerStatusResolver
{
    private readonly IPlayerInput playerInput;
    private readonly IPlayerMovements playerMovements;
    private readonly IPlayerRoll playerRoll;

    public PlayerStatusResolver(IPlayerInput playerInput, IPlayerMovements playerMovements, IPlayerRoll playerRoll)
    {
        this.playerInput = playerInput;
        this.playerMovements = playerMovements;
        this.playerRoll = playerRoll;
    }

    public void ProcessStatusResolver()
    {
        if (playerInput.RunInput && !playerInput.CrouchInput)
        {
            playerMovements.CanCrouch = false;
        }
        else if (playerMovements.IsCrouching)
        {
            playerMovements.CanRun = false;
        }
        else if(!playerInput.RunInput && !playerInput.CrouchInput)
        {
            playerMovements.CanCrouch = true;
            playerMovements.CanRun = true;
        }

        playerMovements.CanTurn = !playerRoll?.IsRolling ?? true;
        playerMovements.IsRunning = playerInput.RunInput && playerMovements.CanRun;
        playerMovements.IsCrouching = playerInput.CrouchInput && playerMovements.CanCrouch;
    }
}
