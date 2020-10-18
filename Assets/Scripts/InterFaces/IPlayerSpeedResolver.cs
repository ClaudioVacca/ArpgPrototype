using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSpeedResolver
{

    float playerSpeedProcessed { get; }
    float SpeedResolver(PlayerStats playerStats, IPlayerInput playerInput, IPlayerMovements playerMovements, IPlayerRoll playerRoll, IPlayerAttacksProcessor playerAttackPRocessor);
}
