using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacksProcessor : MonoBehaviour, IPlayerAttacksProcessor
{
    PlayerController playerController;
    PlayerRoll playerRoll;

    private int _lightAttackCounter = 0;
    private int _heavtAttackCounter = 0;
    private bool _firstComboAttack;
    private bool _isLightAttacking;
    private bool _isHeavyAttacking;
    private bool _canAttack;

    public bool IsLightAttacking
    {
        get { return _isLightAttacking; }
        set { _isLightAttacking = value; }
    }

    public bool IsHeavyAttacking
    {
        get { return _isHeavyAttacking; }
        set { _isHeavyAttacking = value; }
    }

    public bool CanAttack
    {
        get {
            return !playerController.playerMovements.IsCrouching &&
              !playerRoll.IsRolling &&
              !TacticPauseManager.Instance.IsTacticPauseActive &&
              !PlayerAbilityProcessor.Instance.IsExecutingAbility;
                }
        set { _canAttack = value; }
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerRoll = GetComponent<PlayerRoll>();
    }

    public void ProocessAttacks()
    {
        if ((playerController.playerInput.LightAttackInput || playerController.playerInput.HeavyAttackInput) && CanAttack)
        {
            if (_firstComboAttack)
            {
                if (playerController.playerInput.LightAttackInput) IsLightAttacking = true;
                else if (playerController.playerInput.HeavyAttackInput) IsHeavyAttacking = true;
                _firstComboAttack = false;
            }

            if (playerController.playerInput.LightAttackInput) _lightAttackCounter++;
            else if (playerController.playerInput.HeavyAttackInput) _heavtAttackCounter++;
        }
    }

    public void ResetAttackCounter()
    {
        _lightAttackCounter = 0;
        _heavtAttackCounter = 0;
        playerController.playerMovements.CanMove = false;
    }

    public void ManageCombo()
    {
        if (_lightAttackCounter <= 0)
        {
            IsLightAttacking = false;
            _firstComboAttack = true;
        }
        else
            IsLightAttacking = true;

        if (_heavtAttackCounter <= 0)
        {
            IsHeavyAttacking = false;
            _firstComboAttack = true;
        }
        else
            IsHeavyAttacking = true;
    }

    public void AttacksResets()
    {
        IsLightAttacking = false;
        IsHeavyAttacking = false;
        _firstComboAttack = true;
        playerController.playerMovements.CanMove = true;
    }

    void canMoveAndRollEvent()
    {
        playerController.playerMovements.CanMove = true;
    }
}
