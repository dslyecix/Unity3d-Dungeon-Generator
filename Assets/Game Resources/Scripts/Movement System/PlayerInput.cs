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

    //public ParticleSystem jumpParticles;

    private Vector3 _moveInputVector = Vector3.zero;
    private Vector3 _lookInputVector = Vector3.zero;

   
    private new Camera camera;
    private bool canAttack;
    
    private float attackCooldownTimer;

    private void Start()
    {
        attackCooldownTimer = attackCooldown;
        canAttack = true;
        character.IgnoredColliders = IgnoredColliders;

        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
       
        // Gather input
        float moveAxisForward = Input.GetAxisRaw("Vertical");
        float moveAxisRight = Input.GetAxisRaw("Horizontal");

        _moveInputVector = new Vector3(moveAxisRight, 0f, moveAxisForward);
        _moveInputVector = Vector3.ClampMagnitude(_moveInputVector, 1f);

        bool isWalking = Input.GetKey(KeyCode.LeftShift);


        if (Cursor.lockState != CursorLockMode.Locked)
        {
            //_lookInputVector = Vector3.zero;
        }

        if (character)
        {
            // Apply move input to character
            // Vector3 worldSpaceInput = Quaternion.LookRotation(Vector3.forward,Vector3.up) * _moveInputVector;
            // Vector3 lookDirection = new Vector3(worldSpaceInput.x, 0, 0);
            // character.Walk(isWalking);
            // character.SetInputs(worldSpaceInput, lookDirection);
            Plane mousePlane = new Plane(Vector3.up,this.transform.position);
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            float mouseDistanceFromCamera;
            mousePlane.Raycast(cameraRay, out mouseDistanceFromCamera);
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * mouseDistanceFromCamera, Color.red, 0.01f);
            
            Vector3 mouseWorldPosition = cameraRay.origin + cameraRay.direction * mouseDistanceFromCamera;
            mouseWorldPosition.y = transform.position.y;
            Vector3 playerLookDirection = (mouseWorldPosition - this.transform.position).normalized;

            Vector3 worldRelativeInput = Quaternion.LookRotation(Vector3.forward,Vector3.up) * _moveInputVector;
            Vector3 lookDirection = playerLookDirection;
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
                    character.Attack();
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

            // Apply input to virtualCamera
            float scrollInput = -Input.GetAxis("Mouse ScrollWheel");
            #if UNITY_WEBGL
            scrollInput = 0f;
            #endif
        }
    }

    // public void PlayParticles(ParticleSystem _particleSystem) {
    //     _particleSystem.Play(); 
    // }
}
