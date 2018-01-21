using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator playerAnimator;
    private PlayerInput playerInput;

    float strafeAmount;
    float forwardAmount;

    Vector3 moveInput;
    Vector3 lookFacing;
    Vector3 mousePos;

    bool jumpBool;
    bool groundedBool;

    void Start ()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

	void Update ()
    {
        moveInput = playerInput.worldRelativeInput;
        lookFacing = playerInput.lookDirection;
        mousePos = playerInput.mouseWorldPosition; 

        forwardAmount = Vector3.Dot(moveInput, lookFacing);
        strafeAmount = Vector3.Dot(moveInput, playerInput.character.KinematicCharacterMotor.CharacterRight);

        jumpBool = playerInput.character._jumpConsumed;

        groundedBool = playerInput.character.KinematicCharacterMotor.IsStableOnGround;

        UpdateAnimatorValues();
    }

    private void UpdateAnimatorValues()
    {
        playerAnimator.SetFloat("Horizontal Speed", strafeAmount);
        playerAnimator.SetFloat("Forward Speed", forwardAmount);
        playerAnimator.SetBool("Jump", jumpBool);
        playerAnimator.SetBool("IsGrounded", groundedBool);
    }

    // void OnDrawGizmos()
    // {
    //     Debug.DrawRay(transform.position, moveInput, Color.red);
    //     Debug.DrawRay(transform.position, lookFacing, Color.blue);
    //     Debug.DrawLine(transform.position + new Vector3(0, 0.5f, 0), mousePos, Color.magenta);
    // }
}
