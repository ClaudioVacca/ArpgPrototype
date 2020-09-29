using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController playerCC;
    private Animator playerAnimator;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool walking;
    private bool running;
    private float horizontalMov;
    private float verticalMov;
    private bool jump;
    private float playerSpeed = 3.0f;
    private float playerSpeedWhileRunning = 5.0f;
    private float jumpHeight = 1.0f;
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
        ProcessMovement();
        
        
    }

    private void ProcessInput()
    {
        horizontalMov = Input.GetAxis("Horizontal");
        verticalMov = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    private void ProcessStatus()
    {
        groundedPlayer = playerCC.isGrounded;
    }

    private void ProcessMovement()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerAnimator.SetFloat("HorizontalMov", horizontalMov);
        playerAnimator.SetFloat("VerticalMov", verticalMov);

        Vector3 direction = new Vector3(horizontalMov, 0, verticalMov);

        playerCC.Move(direction * Time.deltaTime * playerSpeed);

        if (direction != Vector3.zero)
        {
            gameObject.transform.forward = direction;
        }

        if (jump && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        playerCC.Move(playerVelocity * Time.deltaTime);
    }
}
