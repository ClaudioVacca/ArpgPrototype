using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private PlayerStats playerStats;

    public IPlayerInput playerInput;
    public IPlayerStatusResolver playerStatusResolver;
    public IPlayerGroundChecker playerGroundChecker;
    public IPlayerRoll playerRoll;
    public IPlayerMovements playerMovements;
    public IPlayerAnimatorProcessor playerAnimatorProcessor;

    private CharacterController playerCC;
    private Animator playerAnimator;

    //public IPlayerStatus playerStatus;


    //private Vector3 playerVelocity;
    //private Vector3 direction;



    //[Header("speedStats")]

    //public float playerSpeedWhileRunning = 6f;
    //public float playerSpeedWhileIdleRolling = 3f;
    //public float playerSpeedWhileWalkRolling = 6f;
    //public float playerSpeedWhileRunRolling = 8f;


    //[Header("AttackStats")]
    //public float playerSpeedLightAttacking = 1f;
    //public float playerSpeedHeavyAttacking = 1f;
    //private int lightAttackCounter = 0;
    //private int heavtAttackCounter = 0;
    //private bool firstComboAttack = true;

    //[Header("AnilitiesStats")]
    //private bool doingHurricane;
    //public float HurricaneTime;
    //public float playerSpeedDoingHurricane = 15f;

    //[Header("Utility")]
    //[SerializeField]
    //private float playerSpeedProcessed;
    //[SerializeField]
    //private float gravityValue = -9.81f;
    //private bool onTacticPauseAbilityExecution;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        playerGroundChecker = GetComponent<IPlayerGroundChecker>();
        playerRoll = GetComponent<IPlayerRoll>();
        playerInput = new PlayerInput();
        playerMovements = new PlayerMovements(transform, playerInput, playerGroundChecker, playerRoll, playerCC, playerStats);
        playerStatusResolver = new PlayerStatusResolver(playerInput, playerMovements, playerRoll);
        playerAnimatorProcessor = new PlayerAnimatorProcessor(playerAnimator, playerInput, playerMovements, playerRoll);
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.ProcessInput();
        playerStatusResolver.ProcessStatusResolver();
        playerRoll?.ProcessRoll();
        playerMovements.ProcessMovements();
        playerAnimatorProcessor.ProcessAnimator();

        //ProcessTacticPauseQueue();
        //ProcessAttack();
    }




    //private void ProcessMovement()
    //{
    //    //Direction on x-z axes
    //    direction = new Vector3(playerInput.HorizontalMov, 0, playerInput.VerticalMov);

    //    if (playerStatus.IsRolling || playerStatus.IsLightAttacking || playerStatus.IsHeavyAttacking)
    //        direction = gameObject.transform.forward;

    //    if (direction != Vector3.zero && !playerStatus.IsLightAttacking && !playerStatus.IsHeavyAttacking && !TacticPauseManager.Instance.IsTacticPauseActive)
    //        gameObject.transform.forward = direction;

    //    //Direction on y axe
    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    if (playerStatus.IsGrounded && playerVelocity.y < 0f)
    //    {
    //        playerVelocity.y = 0f;
    //    }

    //    playerSpeedProcessed = SpeedResolver();

    //    if (playerStatus.CanMove)
    //        playerCC.Move(direction.normalized * Time.deltaTime * playerSpeedProcessed);

    //    playerCC.Move(playerVelocity * Time.deltaTime);
    //}

    //void ProcessTacticPauseQueue()
    //{
    //    if (!playerStatus.IsRolling && !playerStatus.IsFalling && !onTacticPauseAbilityExecution && !playerStatus.IsLightAttacking && !playerStatus.IsHeavyAttacking)
    //    {
    //        TacticPauseManager.Instance.DequeueAbility();
    //    }
    //}

    //private void ProcessAttack()
    //{
    //    if ((playerInput.LightAttackInput || playerInput.HeavyAttackInput) && playerStatus.CanAttack && !playerInput.CrouchInput)
    //    {
    //        if (firstComboAttack)
    //        {
    //            if (playerInput.LightAttackInput)
    //            {
    //                playerAnimator.SetBool("LightAttacking", true);
    //                isLightAttacking = true;
    //            }
    //            else if (playerInput.HeavyAttackInput)
    //            {
    //                playerAnimator.SetBool("HeavyAttacking", true);
    //                isHeavyAttacking = true;
    //            }
    //            firstComboAttack = false;
    //        }

    //        if (playerInput.LightAttackInput)
    //            lightAttackCounter++;
    //        else if (playerInput.HeavyAttackInput)
    //            heavtAttackCounter++;
    //    }
    //}

    

    //private float SpeedResolver()
    //{
    //    playerSpeedProcessed = playerStats.Speed;
    //    if (playerInput.RunInput && playerInput.CrouchInput)
    //    {

    //        if (playerStatus.FirstIsRunning)
    //            playerSpeedProcessed = playerInput.RunInput ? playerSpeedWhileRunning : playerSpeedProcessed;
    //        else
    //            playerSpeedProcessed = playerInput.CrouchInput ? playerSpeedProcessed / 2 : playerSpeedProcessed;
    //    }
    //    else
    //    {
    //        playerSpeedProcessed = playerInput.RunInput ? playerSpeedWhileRunning : playerSpeedProcessed;
    //        playerSpeedProcessed = playerInput.CrouchInput ? playerSpeedProcessed / 2 : playerSpeedProcessed;
    //    }

    //    playerSpeedProcessed = Math.Abs(playerInput.HorizontalMov) > 0 && Math.Abs(playerInput.VerticalMov) > 0 ? playerSpeedProcessed * 0.75f : playerSpeedProcessed;

    //    if (IsMoving)
    //    {
    //        if (playerInput.RunInput)
    //            playerSpeedProcessed = playerStatus.IsRolling ? playerSpeedWhileRunRolling : playerSpeedProcessed;
    //        else
    //            playerSpeedProcessed = playerStatus.IsRolling ? playerSpeedWhileWalkRolling : playerSpeedProcessed;
    //    }
    //    else
    //        playerSpeedProcessed = playerStatus.IsRolling ? playerSpeedWhileIdleRolling : playerSpeedProcessed;

    //    if (playerStatus.IsLightAttacking)
    //        playerSpeedProcessed = playerSpeedLightAttacking;
    //    if (playerStatus.IsHeavyAttacking)
    //        playerSpeedProcessed = playerSpeedHeavyAttacking;

    //    if (doingHurricane)
    //        playerSpeedProcessed = playerSpeedDoingHurricane;

    //    return playerSpeedProcessed;
    //}

    //public void ResetAttackCounter()
    //{
    //    lightAttackCounter = 0;
    //    heavtAttackCounter = 0;
    //    canMove = false;
    //    canRoll = false;
    //}

    //public void ManageCombo()
    //{
    //    if (lightAttackCounter <= 0)
    //    {
    //        playerAnimator.SetBool("LightAttacking", false);
    //        isLightAttacking = false;
    //        firstComboAttack = true;
    //    }
    //    else
    //    {
    //        isLightAttacking = true;
    //        playerAnimator.SetBool("LightAttacking", true);
    //    }

    //    if (heavtAttackCounter <= 0)
    //    {
    //        playerAnimator.SetBool("HeavyAttacking", false);
    //        isHeavyAttacking = false;
    //        firstComboAttack = true;
    //    }
    //    else
    //    {
    //        isHeavyAttacking = true;
    //        playerAnimator.SetBool("HeavyAttacking", true);
    //    }

    //}

    //void AttacksResets()
    //{
    //    isLightAttacking = false;
    //    playerAnimator.SetBool("LightAttacking", false);
    //    isHeavyAttacking = false;
    //    playerAnimator.SetBool("HeavyAttacking", false);
    //    firstComboAttack = true;
    //    canMove = true;
    //    canRoll = true;
    //}

    //void canMoveAndRollEvent()
    //{
    //    canMove = true;
    //    canRoll = true;
    //}

    //public void HurricanAbilityUsed()
    //{
    //    StartCoroutine(HurricaneCoroutine());
    //}

    //private IEnumerator HurricaneCoroutine()
    //{
    //    onTacticPauseAbilityExecution = true;
    //    doingHurricane = true;
    //    canRoll = false;
    //    canAttack = false;
    //    AttacksResets();
    //    playerAnimator.SetBool("Hurricane", true);
    //    yield return new WaitForSeconds(HurricaneTime);
    //    canRoll = true;
    //    canAttack = true;
    //    playerAnimator.SetBool("Hurricane", false);
    //    doingHurricane = false;
    //    onTacticPauseAbilityExecution = false;
    //}
}
