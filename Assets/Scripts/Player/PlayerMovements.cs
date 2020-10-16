using System;
using UnityEngine;

public class PlayerMovements : IPlayerMovements
{
    private IPlayerSpeedResolver playerSpeedResolver = new PlayerSpeedResolver();
    private readonly IPlayerInput playerInput;
    private readonly IPlayerGroundChecker playerGroundChecker;
    private readonly IPlayerRoll playerRoll;
    private readonly CharacterController cc;
    private readonly PlayerStats playerStats;
    private readonly Transform playerTransform;
    private Vector3 playerVelocity;
    private float playerSpeedProcessed;
    float gravityValue = -9.81f;

    public Vector3 Direction { get; set; }

    private bool _isMoving;
    private bool _canRun;
    private bool _canCrouch;
    private bool _canTurn = true;
    private bool _wasRunningFirst;

    public bool IsMoving
    {
        get
        {
            return Math.Abs(playerInput.HorizontalMov) > 0 || Math.Abs(playerInput.VerticalMov) > 0;
        }
        set { _isMoving = value; }
    }

    public bool CanMove { get; set; } = true;

    public bool CanTurn { get; set; } = true;

    public bool CanRun { get; set; }

    public bool CanCrouch { get; set; }

    public bool IsRunning { get; set; }

    public bool IsCrouching { get; set; }

    public PlayerMovements(Transform playerTransform, IPlayerInput playerInput, IPlayerGroundChecker playerGroundChecker, IPlayerRoll playerRoll, CharacterController cc, PlayerStats playerStats)
    {
        this.playerTransform = playerTransform;
        this.playerInput = playerInput;
        this.playerGroundChecker = playerGroundChecker;
        this.playerRoll = playerRoll;
        this.cc = cc;
        this.playerStats = playerStats;
        playerStats.CurrentSpeed = playerStats.InitialSpeed;
    }

    public void ProcessMovements()
    {
        //Direction on x-z axes
        Direction = new Vector3(playerInput.HorizontalMov, 0, playerInput.VerticalMov);

        if (Direction != Vector3.zero && CanTurn /*&& !playerStatus.IsLightAttacking && !playerStatus.IsHeavyAttacking && !TacticPauseManager.Instance.IsTacticPauseActive*/)
            playerTransform.forward = Direction;

        if (!CanTurn /*|| playerStatus.IsLightAttacking || playerStatus.IsHeavyAttacking*/)
            Direction = playerTransform.forward;

        //Direction on y axe
        playerVelocity.y += gravityValue * Time.deltaTime;

        if (playerGroundChecker.IsGrounded() && playerVelocity.y < 0f)
        {
            playerVelocity.y = 0f;
        }

        playerSpeedProcessed = playerSpeedResolver.SpeedResolver(playerStats, playerInput, this, playerRoll);

        if (CanMove)
            cc.Move(Direction.normalized * Time.deltaTime * playerStats.CurrentSpeed);

        cc.Move(playerVelocity * Time.deltaTime);
    }
}
