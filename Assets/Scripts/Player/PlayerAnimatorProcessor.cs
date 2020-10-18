using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorProcessor : IPlayerAnimatorProcessor
{
    Animator animator;
    IPlayerGroundChecker playerGroundChecker;
    IPlayerInput input;
    IPlayerMovements playerMovements;
    IPlayerRoll playerRoll;
    IPlayerAttacksProcessor playerAttacksProcessor;

    private int horizontalMov = Animator.StringToHash("HorizontalMov");
    private int verticalMov = Animator.StringToHash("VerticalMov");
    private int moving = Animator.StringToHash("Moving");
    private int running = Animator.StringToHash("Running");
    private int crouching = Animator.StringToHash("Crouching");
    private int rolling = Animator.StringToHash("Rolling");
    private int falling = Animator.StringToHash("Falling");
    private int lightAttacking = Animator.StringToHash("LightAttacking");
    private int heavyAttacking = Animator.StringToHash("HeavyAttacking");
    private int hurricane = Animator.StringToHash("Hurricane");

    public PlayerAnimatorProcessor(Animator animator,IPlayerGroundChecker playerGroundChecker, IPlayerInput input, IPlayerMovements playerMovements, IPlayerRoll playerRoll, IPlayerAttacksProcessor playerAttacksProcessor)
    {
        this.animator = animator;
        this.input = input;
        this.playerMovements = playerMovements;
        this.playerRoll = playerRoll;
        this.playerAttacksProcessor = playerAttacksProcessor;
        this.playerGroundChecker = playerGroundChecker;
    }

    public void ProcessAnimator()
    {
        animator.SetFloat(horizontalMov, input.HorizontalMov);
        animator.SetFloat(verticalMov, input.VerticalMov);
        animator.SetBool(moving, playerMovements.IsMoving);
        animator.SetBool(rolling, playerRoll.IsRolling);
        animator.SetBool(running, playerMovements.IsRunning);
        animator.SetBool(crouching, playerMovements.IsCrouching);
        animator.SetBool(falling, !playerGroundChecker.IsGrounded());
        animator.SetBool(lightAttacking, playerAttacksProcessor.IsLightAttacking);
        animator.SetBool(heavyAttacking, playerAttacksProcessor.IsHeavyAttacking);
    }

    public void SetHurricane(bool value)
    {
        animator.SetBool(hurricane, value);
    }
}
