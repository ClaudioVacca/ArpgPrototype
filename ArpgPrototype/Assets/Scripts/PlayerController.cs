using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController playerCC;
    private Animator playerAnimator;
    private Vector3 playerVelocity;
    private Vector3 direction;

    private bool groundedPlayer;
    private bool running;
    private bool crouching;
    private bool firstIsRunning;
    private float horizontalMov;
    private float verticalMov;
    private bool roll;
    private bool rolling;
    private bool canRoll = true;
    private bool falling;

    [Header("Stats")]
    public float playerSpeed = 4f;
    public float playerSpeedWhileRunning = 6f;
    public float playerSpeedWhileIdleRolling = 3f;
    public float playerSpeedWhileRunRolling = 6f;
    public float idleRollTime = 2f;
    public float runRollTime = 1f;
    public float rollCooldown = 2f;
    [SerializeField]
    private float playerSpeedProcessed;
    [SerializeField]
    private float gravityValue = -9.81f;



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
        ProcessAnimator();
        ProcessMovement();
    }

    private void ProcessInput()
    {
        horizontalMov = Input.GetAxis("Horizontal");
        verticalMov = Input.GetAxis("Vertical");
        running = Input.GetKey(KeyCode.LeftShift) ? true : false;
        crouching = Input.GetKey(KeyCode.C) ? true : false;
        roll = Input.GetKeyDown(KeyCode.Space) ? true : false;
    }

    private void ProcessStatus()
    {
        groundedPlayer = playerCC.isGrounded;
        Debug.Log(groundedPlayer);

        if (roll && canRoll)
        {
            if (Math.Abs(verticalMov) > 0 || Math.Abs(horizontalMov) > 0)
                StartCoroutine(RollingCoroutine(runRollTime));
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
    }

    private void ProcessAnimator()
    {
        playerAnimator.SetFloat("HorizontalMov", horizontalMov);
        playerAnimator.SetFloat("VerticalMov", verticalMov);
        playerAnimator.SetBool("Running", running);
        playerAnimator.SetBool("Crouching", crouching);
        playerAnimator.SetBool("Rolling", rolling);
        playerAnimator.SetBool("Falling", !groundedPlayer);
    }

    private void ProcessMovement()
    {
        //Direction on x-z axes
        if (!rolling)
            direction = new Vector3(horizontalMov, 0, verticalMov);

        if (direction != Vector3.zero)
            gameObject.transform.forward = direction;

        //Direction on y axe
        playerVelocity.y += gravityValue * Time.deltaTime;
        //if (groundedPlayer && playerVelocity.y < 0f)
        //{
        //    playerVelocity.y = 0f;
        //}

        //SpeedResolver
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
        playerSpeedProcessed = (Math.Abs(direction.x) > 0 && Math.Abs(direction.z) > 0) ? playerSpeedProcessed * 0.75f : playerSpeedProcessed;

        if (Math.Abs(verticalMov) > 0 || Math.Abs(horizontalMov) > 0)
            playerSpeedProcessed = rolling ? playerSpeedWhileRunRolling : playerSpeedProcessed;
        else
            playerSpeedProcessed = rolling ? playerSpeedWhileIdleRolling : playerSpeedProcessed;

        if (rolling)
            direction = gameObject.transform.forward;

        playerCC.Move(direction * Time.deltaTime * playerSpeedProcessed);
        playerCC.Move(playerVelocity * Time.deltaTime);
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

    //void RollEnd()
    //{
    //    rolling = false;
    //    StartCoroutine(RollCD());
    //}

    //IEnumerator RollCD()
    //{
    //    yield return new WaitForSeconds(rollCooldown);
    //    canRoll = true;
    //}
}
