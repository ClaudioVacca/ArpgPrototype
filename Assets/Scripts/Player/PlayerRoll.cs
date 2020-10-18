using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerRoll : MonoBehaviour, IPlayerRoll
{
    PlayerController playerController;

    private bool _canRoll;
    private bool _isRolling;
    private bool _isRollInCoolDown;

    public float idleRollDuration = 2f;
    public float movingRollDuration = 1f;
    public float rollCooldown = 2f;

    public bool CanRoll
    {
        get
        {
            return !playerController.playerInput.CrouchInput &&
                !IsRolling &&
                !IsRollInCooldown &&
                !playerController.playerAttacksProcessor.IsLightAttacking &&
                !playerController.playerAttacksProcessor.IsHeavyAttacking &&
                !TacticPauseManager.Instance.IsTacticPauseActive && 
                !PlayerAbilityProcessor.Instance.IsExecutingAbility;
        }
        set { _canRoll = value; }
    }

    public bool IsRolling { get; set; }

    public bool IsRollInCooldown { get; set; }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void ProcessRoll()
    {
        if (playerController.playerInput.RollInput && CanRoll)
        {
            if (!playerController.playerMovements.IsMoving)
                StartCoroutine(RollingCoroutine(idleRollDuration));
            else
                StartCoroutine(RollingCoroutine(movingRollDuration));
        }
    }

    IEnumerator RollingCoroutine(float rollTime)
    {
        IsRollInCooldown = true;
        IsRolling = true;
        yield return new WaitForSeconds(rollTime);
        IsRolling = false;
        yield return new WaitForSeconds(rollCooldown - rollTime);
        IsRollInCooldown = false;
    }
}
