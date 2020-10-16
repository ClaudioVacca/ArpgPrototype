using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorProcessor : IPlayerAnimatorProcessor
{
    Animator animator;
    IPlayerInput input;
    IPlayerMovements playerMovements;
    IPlayerRoll playerRoll;

    private int horizontalMov = Animator.StringToHash("HorizontalMov");
    private int verticalMov = Animator.StringToHash("VerticalMov");
    private int moving = Animator.StringToHash("Moving");
    private int rolling = Animator.StringToHash("Rolling");

    public PlayerAnimatorProcessor(Animator animator, IPlayerInput input, IPlayerMovements playerMovements, IPlayerRoll playerRoll)
    {
        this.animator = animator;
        this.input = input;
        this.playerMovements = playerMovements;
        this.playerRoll = playerRoll;
    }

    public void ProcessAnimator()
    {
        animator.SetFloat(horizontalMov, input.HorizontalMov);
        animator.SetFloat(verticalMov, input.VerticalMov);
        animator.SetBool(moving, playerMovements.IsMoving);
        animator.SetBool(rolling, playerRoll.IsRolling);
        animator.SetBool("Running", playerMovements.IsRunning);
        animator.SetBool("Crouching", playerMovements.IsCrouching);

        //animator.SetBool("Falling", !input.playerStatus.IsGrounded);
    }
}
