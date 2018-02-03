using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;

public class PlayerInput : MonoBehaviour
{
    public PlayerCharacterController character;
    public float MouseSensitivity = 0.02f;
    public float attackCooldown = 5;
    public Collider[] IgnoredColliders;
    public Vector3 worldRelativeInput;
    public Vector3 lookDirection;
    public Vector3 mouseWorldPosition;

    private Vector3 _moveInputVector = Vector3.zero;
    private Vector3 _lookInputVector = Vector3.zero;
    private new Camera camera;
    private bool canAttack;
    private float attackCooldownTimer;

    PlayerAnimationController playerAnimationController;

    private void Start()
    {
        attackCooldownTimer = attackCooldown;
        canAttack = true;
        character.IgnoredColliders = IgnoredColliders;

        camera = FindObjectOfType<Camera>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
       
        // Gather input
        float moveAxisForward = Input.GetAxisRaw("Vertical");
        float moveAxisRight = Input.GetAxisRaw("Horizontal");

        _moveInputVector = new Vector3(moveAxisRight, 0f, moveAxisForward);
        _moveInputVector = Vector3.ClampMagnitude(_moveInputVector, 1f);

        bool isWalking = Input.GetKey(KeyCode.LeftShift);

        if (character)
        {
            worldRelativeInput = Vector3.zero;
            // Apply move input to character
            // Vector3 worldSpaceInput = Quaternion.LookRotation(Vector3.forward,Vector3.up) * _moveInputVector;
            // Vector3 lookDirection = new Vector3(worldSpaceInput.x, 0, 0);
            // character.Walk(isWalking);
            // character.SetInputs(worldSpaceInput, lookDirection);
            Plane characterPlane = new Plane(Vector3.up,character.transform.position);
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            float mouseDistanceFromCamera;
            characterPlane.Raycast(cameraRay, out mouseDistanceFromCamera);
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * mouseDistanceFromCamera, Color.red, 0.01f);
            
            mouseWorldPosition = cameraRay.origin + cameraRay.direction * mouseDistanceFromCamera;
            mouseWorldPosition.y = transform.position.y;
            Vector3 playerLookDirection = (mouseWorldPosition - this.transform.position).normalized;

            worldRelativeInput = Quaternion.LookRotation(Vector3.forward,Vector3.up) * _moveInputVector;
            lookDirection = playerLookDirection;
            character.Walk(isWalking);
            character.SetInputs(worldRelativeInput, lookDirection);

            // Jump input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                character.Jump();
            }

            if (!canAttack) {
                attackCooldownTimer -= Time.deltaTime;
                if (attackCooldownTimer <=0f) canAttack = true;
            } 

            if (Input.GetKeyDown(KeyCode.Q))
            {
                character.DoAFlip();
            }


            //Attack input?
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canAttack) {
                    playerAnimationController.Attack();
                    attackCooldownTimer = attackCooldown;
                    canAttack = false;
                } 
            }

            // Croucing input
            if (Input.GetKeyDown(KeyCode.C))
            {
                character.Crouch(true);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                character.Crouch(false);
            }
        }
    }
}
