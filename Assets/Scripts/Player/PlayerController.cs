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
    public IPlayerAttacksProcessor playerAttacksProcessor;
    public IPlayerMovements playerMovements;
    public IPlayerAnimatorProcessor playerAnimatorProcessor;

    private CharacterController playerCC;
    private Animator playerAnimator;

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
        playerAttacksProcessor = GetComponent<IPlayerAttacksProcessor>();
        playerInput = new PlayerInput();
        playerMovements = new PlayerMovements(transform, playerInput, playerGroundChecker, playerRoll, playerAttacksProcessor, playerCC, playerStats);
        playerStatusResolver = new PlayerStatusResolver(playerInput, playerMovements, playerRoll);
        playerAnimatorProcessor = new PlayerAnimatorProcessor(playerAnimator, playerGroundChecker, playerInput, playerMovements, playerRoll, playerAttacksProcessor);
    }

    void Update()
    {
        playerInput.ProcessInput();
        playerStatusResolver.ProcessStatusResolver();
        playerMovements.ProcessMovements();
        playerAnimatorProcessor.ProcessAnimator();
        playerRoll.ProcessRoll();
        playerAttacksProcessor.ProocessAttacks();
        ProcessAbilities();

        //ProcessTacticPauseQueue();
    }

    private void ProcessAbilities()
    {
        if (playerGroundChecker.IsGrounded() &&
            !playerRoll.IsRolling &&
            !playerAttacksProcessor.IsLightAttacking &&
            !playerAttacksProcessor.IsHeavyAttacking &&
            !PlayerAbilityProcessor.Instance.IsExecutingAbility
            )
            PlayerAbilityProcessor.Instance.DequeueAndUseAbility();
    }

    //void ProcessTacticPauseQueue()
    //{
    //    if (!playerStatus.IsRolling && !playerStatus.IsFalling && !onTacticPauseAbilityExecution && !playerStatus.IsLightAttacking && !playerStatus.IsHeavyAttacking)
    //    {
    //        TacticPauseManager.Instance.DequeueAbility();
    //    }
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
