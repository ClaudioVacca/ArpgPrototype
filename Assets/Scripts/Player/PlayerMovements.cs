using System;
using UnityEngine;

public class PlayerMovements : IPlayerMovements
{
    private IPlayerSpeedResolver playerSpeedResolver = new PlayerSpeedResolver();
    private readonly IPlayerInput playerInput;
    private readonly IPlayerGroundChecker playerGroundChecker;
    private readonly IPlayerRoll playerRoll;
    private readonly IPlayerAttacksProcessor playerAttacksProcessor;
    private readonly CharacterController cc;
    private readonly PlayerStats playerStats;
    private readonly Transform playerTransform;
    private Vector3 DirectionOnY;
    private float playerSpeedProcessed;
    float gravityValue = -9.81f;

    public Vector3 DirectionOnXZ { get; set; }

    private bool _isMoving;
    private bool _canMove;
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

    public bool CanMove {
        get
        {
            return _canMove;
        }
        set
        {
            _canMove = value;
        }
    }

    public bool CanTurn { get; set; } = true;

    public bool CanRun { get; set; }

    public bool CanCrouch { get; set; }

    public bool IsRunning { get; set; }

    public bool IsCrouching { get; set; }

    public PlayerMovements(
        Transform playerTransform, 
        IPlayerInput playerInput, 
        IPlayerGroundChecker playerGroundChecker, 
        IPlayerRoll playerRoll,
        IPlayerAttacksProcessor playerAttacksProcessor, 
        CharacterController cc, 
        PlayerStats playerStats)
    {
        this.playerTransform = playerTransform;
        this.playerInput = playerInput;
        this.playerGroundChecker = playerGroundChecker;
        this.playerRoll = playerRoll;
        this.playerAttacksProcessor = playerAttacksProcessor;
        this.cc = cc;
        this.playerStats = playerStats;
        playerStats.CurrentSpeed = playerStats.InitialSpeed;
    }

    public void ProcessMovements()
    {
        //Direction on x-z axes
        DirectionOnXZ = new Vector3(playerInput.HorizontalMov, 0, playerInput.VerticalMov);

        if (DirectionOnXZ != Vector3.zero && CanTurn && !playerAttacksProcessor.IsLightAttacking && !playerAttacksProcessor.IsHeavyAttacking && !TacticPauseManager.Instance.IsTacticPauseActive)
            playerTransform.forward = DirectionOnXZ;

        if (!CanTurn || playerAttacksProcessor.IsLightAttacking || playerAttacksProcessor.IsHeavyAttacking)
            DirectionOnXZ = playerTransform.forward;

        //Direction on y axe
        DirectionOnY.y += gravityValue * Time.deltaTime;

        if (playerGroundChecker.IsGrounded() && DirectionOnY.y < 0f)
        {
            DirectionOnY.y = 0f;
        }

        //Speed retrieving
        playerSpeedProcessed = playerSpeedResolver.SpeedResolver(playerStats, playerInput, this, playerRoll, playerAttacksProcessor);

        //Movements
        if (CanMove)
            cc.Move(DirectionOnXZ.normalized * Time.deltaTime * playerStats.CurrentSpeed);

        cc.Move(DirectionOnY * Time.deltaTime);
    }
}
