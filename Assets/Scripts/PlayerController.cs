﻿using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    Player player;
    private CharacterController playerCC;
    private Animator playerAnimator;
    private Vector3 playerVelocity;
    private Vector3 direction;

    [Header("Utility")]
    public Transform _groundChecker;
    private float groundCheckerRadius = 0.2f;
    [SerializeField]
    private float playerSpeedProcessed;
    [SerializeField]
    private float gravityValue = -9.81f;
    private bool onTacticPauseAbilityExecution;

    private bool isGrounded;
    private bool canMove = true;
    private bool moving;
    private bool running;
    private bool crouching;
    private bool firstIsRunning;
    private float horizontalMov;
    private float verticalMov;
    private bool roll;
    private bool rolling;
    private bool canRoll = true;
    private bool falling;

    [Header("RollStats")]
    public float playerSpeed = 4f;
    public float playerSpeedWhileRunning = 6f;
    public float playerSpeedWhileIdleRolling = 3f;
    public float playerSpeedWhileWalkRolling = 6f;
    public float playerSpeedWhileRunRolling = 8f;
    public float idleRollTime = 2f;
    public float walkRollTime = 1f;
    public float rollCooldown = 2f;

    [Header("AttackStats")]
    public float playerSpeedLightAttacking = 1f;
    public float playerSpeedHeavyAttacking = 1f;
    private bool lightAttack;
    private bool heavyAttack;
    private bool canAttack = true;
    private bool lightAttacking;
    private bool heavyAttacking;
    private int lightAttackCounter = 0;
    private int heavtAttackCounter = 0;
    private bool firstComboAttack = true;

    [Header("AnilitiesStats")]
    private bool doingHurricane;
    public float HurricaneTime;
    public float playerSpeedDoingHurricane = 15f;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessStatus();
        ProcessTacticPauseQueue();
        ProcessMovement();
        ProcessAttack();
        ProcessAnimator();
    }

    private void ProcessInput()
    {
        //horizontalMov = Input.GetAxis("Horizontal");
        //verticalMov = Input.GetAxis("Vertical");
        //running = Input.GetKey(KeyCode.LeftShift) ? true : false;
        //crouching = Input.GetKey(KeyCode.C) ? true : false;
        //roll = Input.GetKeyDown(KeyCode.Space) ? true : false;
        //if (!EventSystem.current.IsPointerOverGameObject() && !TacticPauseManager.Instance.IsTacticPauseActive)
        //{
        //    lightAttack = Input.GetKeyDown(KeyCode.Mouse0);
        //    heavyAttack = Input.GetKeyDown(KeyCode.Mouse1);
        //}

        horizontalMov = player.GetAxis("HorizontalMov");
        verticalMov = player.GetAxis("VerticalMov");
        running = player.GetButton("Run") ? true : false;
        crouching = player.GetButton("Crouch") ? true : false;
        roll = player.GetButtonDown("Roll") ? true : false;

        if (!EventSystem.current.IsPointerOverGameObject() && !TacticPauseManager.Instance.IsTacticPauseActive)
        {
            lightAttack = player.GetButtonDown("LightAttack");
            heavyAttack = player.GetButtonDown("HeavyAttack");
        }

        TacticPauseManager.Instance.tacticPause = player.GetButtonDown("TacticPause");
        if (TacticPauseManager.Instance.tacticPause)
            TacticPauseManager.Instance.IsTacticPauseActive = !TacticPauseManager.Instance.IsTacticPauseActive;


    }

    private void ProcessStatus()
    {
        if (Math.Abs(horizontalMov) > 0 || Math.Abs(verticalMov) > 0)
            moving = true;
        else
            moving = false;

        isGrounded = Physics.CheckSphere(_groundChecker.position, groundCheckerRadius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
        canAttack = !isGrounded || rolling ? false : true;

        if (roll && canRoll && !crouching && !lightAttacking && !heavyAttacking)
        {
            if (moving)
                StartCoroutine(RollingCoroutine(walkRollTime));
            else
                StartCoroutine(RollingCoroutine(idleRollTime));
        }

        if (running ^ crouching)
        {
            if (running)
                firstIsRunning = true;
            else
                firstIsRunning = false;
        }

        if (lightAttacking || heavyAttacking)
        {
            crouching = false;
        }
    }

    void ProcessTacticPauseQueue()
    {
        if (!rolling && !falling && !onTacticPauseAbilityExecution && !lightAttacking && !heavyAttacking)
        {
            TacticPauseManager.Instance.DequeueAbility();
        }
    }


    private void ProcessMovement()
    {
        //Direction on x-z axes
        direction = new Vector3(horizontalMov, 0, verticalMov);

        if (rolling || lightAttacking || heavyAttacking)
            direction = gameObject.transform.forward;

        if (direction != Vector3.zero && !lightAttacking && !heavyAttacking && !TacticPauseManager.Instance.IsTacticPauseActive)
            gameObject.transform.forward = direction;

        //Direction on y axe
        playerVelocity.y += gravityValue * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = 0f;
        }

        playerSpeedProcessed = SpeedResolver();

        if (canMove)
            playerCC.Move(direction * Time.deltaTime * playerSpeedProcessed);

        playerCC.Move(playerVelocity * Time.deltaTime);
    }

    private void ProcessAttack()
    {
        if ((lightAttack || heavyAttack) && canAttack && !crouching)
        {
            if (firstComboAttack)
            {
                if (lightAttack)
                {
                    playerAnimator.SetBool("LightAttacking", true);
                    lightAttacking = true;
                }
                else if (heavyAttack)
                {
                    playerAnimator.SetBool("HeavyAttacking", true);
                    heavyAttacking = true;
                }
                firstComboAttack = false;
            }

            if (lightAttack)
                lightAttackCounter++;
            else if (heavyAttack)
                heavtAttackCounter++;
        }
    }

    private void ProcessAnimator()
    {
        playerAnimator.SetFloat("HorizontalMov", horizontalMov);
        playerAnimator.SetFloat("VerticalMov", verticalMov);
        playerAnimator.SetBool("Moving", moving);
        playerAnimator.SetBool("Running", running);
        playerAnimator.SetBool("Crouching", crouching);
        playerAnimator.SetBool("Rolling", rolling);
        playerAnimator.SetBool("Falling", !isGrounded);
    }

    private float SpeedResolver()
    {
        playerSpeedProcessed = playerSpeed;
        if (running && crouching)
        {

            if (firstIsRunning)
                playerSpeedProcessed = running ? playerSpeedWhileRunning : playerSpeedProcessed;
            else
                playerSpeedProcessed = crouching ? playerSpeedProcessed / 2 : playerSpeedProcessed;
        }
        else
        {
            playerSpeedProcessed = running ? playerSpeedWhileRunning : playerSpeedProcessed;
            playerSpeedProcessed = crouching ? playerSpeedProcessed / 2 : playerSpeedProcessed;
        }

        playerSpeedProcessed = Math.Abs(horizontalMov) > 0 && Math.Abs(verticalMov) > 0 ? playerSpeedProcessed * 0.75f : playerSpeedProcessed;

        if (moving)
        {
            if (running)
                playerSpeedProcessed = rolling ? playerSpeedWhileRunRolling : playerSpeedProcessed;
            else
                playerSpeedProcessed = rolling ? playerSpeedWhileWalkRolling : playerSpeedProcessed;
        }
        else
            playerSpeedProcessed = rolling ? playerSpeedWhileIdleRolling : playerSpeedProcessed;

        if (lightAttacking)
            playerSpeedProcessed = playerSpeedLightAttacking;
        if (heavyAttacking)
            playerSpeedProcessed = playerSpeedHeavyAttacking;

        if (doingHurricane)
            playerSpeedProcessed = playerSpeedDoingHurricane;

        return playerSpeedProcessed;
    }

    IEnumerator RollingCoroutine(float rollTime)
    {
        canRoll = false;
        rolling = true;
        yield return new WaitForSeconds(rollTime);
        rolling = false;
        yield return new WaitForSeconds(rollCooldown - rollTime);
        canRoll = true;
    }

    public void ResetAttackCounter()
    {
        lightAttackCounter = 0;
        heavtAttackCounter = 0;
        canMove = false;
        canRoll = false;
    }

    public void ManageCombo()
    {
        if (lightAttackCounter <= 0)
        {
            playerAnimator.SetBool("LightAttacking", false);
            lightAttacking = false;
            firstComboAttack = true;
        }
        else
        {
            lightAttacking = true;
            playerAnimator.SetBool("LightAttacking", true);
        }

        if (heavtAttackCounter <= 0)
        {
            playerAnimator.SetBool("HeavyAttacking", false);
            heavyAttacking = false;
            firstComboAttack = true;
        }
        else
        {
            heavyAttacking = true;
            playerAnimator.SetBool("HeavyAttacking", true);
        }

    }

    void AttacksResets()
    {
        lightAttacking = false;
        playerAnimator.SetBool("LightAttacking", false);
        heavyAttacking = false;
        playerAnimator.SetBool("HeavyAttacking", false);
        firstComboAttack = true;
        canMove = true;
        canRoll = true;
    }

    void canMoveAndRollEvent()
    {
        canMove = true;
        canRoll = true;
    }

    public void HurricanAbilityUsed()
    {
        StartCoroutine(HurricaneCoroutine());
    }

    private IEnumerator HurricaneCoroutine()
    {
        onTacticPauseAbilityExecution = true;
        doingHurricane = true;
        canRoll = false;
        canAttack = false;
        AttacksResets();
        playerAnimator.SetBool("Hurricane", true);
        yield return new WaitForSeconds(HurricaneTime);
        canRoll = true;
        canAttack = true;
        playerAnimator.SetBool("Hurricane", false);
        doingHurricane = false;
        onTacticPauseAbilityExecution = false;
    }
}
