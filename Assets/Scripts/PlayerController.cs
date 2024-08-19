using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;

    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float rotationSpeed = 90f;
    public float drag = 0.1f;
    public float angularDrag = 0.1f;

    [Header("Camera Settings")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 89f;
    private CinemachineOrbitalTransposer orbitalTransposer;

    private PlayerLocomotionInput _playerLocomotionInput;
    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;


    [Header("Jumping")]
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool jumpReleased;

    [Header("Attack")]
    public Transform batPivot;
    public bool isSwinging;
    //private Quaternion originQuat;
    private Quaternion targetQuat;
    public float swingTargetA;
    public float swingTargetB;
    public bool isTargetingA;
    public float attackSpeed;
    public float diagonalAngle = 30f;

    [Header("Stats")]
    public float initialScale = 0.4f;
    public float damage = 1f;
    public float growthFactor = 1.1f;

    private bool isGrowing;
    public float growthRate = 10f;
    private Vector3 targetScale;
    private float targetGroundDistance;
    private float targetCameraFollowZ;

    [Header("Sounds")]
    public AudioSource sndBatSwing;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        //originQuat = batPivot.rotation;
        targetQuat = Quaternion.Euler(0f, swingTargetB, -diagonalAngle);
        orbitalTransposer = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        // Attack!
        if(!isSwinging && _playerLocomotionInput.GetAttack.WasPressedThisFrame())
        {
            isSwinging = true;
            sndBatSwing.Play();
        }
        else if(isSwinging)
        {
            batPivot.localRotation = Quaternion.Lerp(batPivot.localRotation, targetQuat, attackSpeed * Time.deltaTime);
            
            if (Quaternion.Angle(batPivot.localRotation, targetQuat) < 1f)
            {
                isSwinging = false;
                isTargetingA = !isTargetingA;
                if (isTargetingA)
                {
                    targetQuat = Quaternion.Euler(0f, swingTargetA, diagonalAngle);
                }
                else
                {
                    targetQuat = Quaternion.Euler(0f, swingTargetB, -diagonalAngle);
                }
            }
        }


        // Rotate character
        _playerTargetRotation.x += transform.eulerAngles.x + rotationSpeed * _playerLocomotionInput.RotateInput.x * angularDrag * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        if (!jumpReleased && _playerLocomotionInput.GetJump.WasReleasedThisFrame())
        {
            jumpReleased = true;
        }

        // Check for ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) && jumpReleased;

        Vector3 horizontalVelocity = _characterController.velocity;
        float verticalVelocity = _characterController.velocity.y;

        if (isGrounded)
        {
            // Add velocity
            Vector3 movementDelta = _playerLocomotionInput.MovementInput.y * transform.forward * runAcceleration * Time.deltaTime;
            horizontalVelocity = _characterController.velocity + movementDelta;

            // Add drag to player
            Vector3 currentDrag = horizontalVelocity.normalized * drag * Time.deltaTime;
            horizontalVelocity = (horizontalVelocity.magnitude > drag * Time.deltaTime) ? horizontalVelocity - currentDrag : Vector3.zero;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, runSpeed);
        
            // reset velocity and jump
            if(verticalVelocity < 0)
            {
                verticalVelocity = -2f;

                if (jumpReleased)
                {
                    jumpReleased = false;
                }
            }
        }

        // Jump
        if (_playerLocomotionInput.GetJump.IsPressed() && isGrounded && jumpReleased)
        {
            verticalVelocity += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Reduce upward velocity when the jump button is released early
        else if (!_playerLocomotionInput.GetJump.IsPressed())
        {
            jumpReleased = true;
            if (verticalVelocity > 0) { 
                verticalVelocity *= 0.5f;
            }
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Move character (Unity suggests calling this only once per frame)
        Vector3 newVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        _characterController.Move(newVelocity * Time.deltaTime);

        Grow();
    }

    private void LateUpdate()
    {

        _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
    }

    public void Grow()
    {
        
        if(isGrowing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthRate * Time.deltaTime);
            groundDistance = Mathf.Lerp(groundDistance, targetGroundDistance, growthRate * Time.deltaTime);
            orbitalTransposer.m_FollowOffset.z = Mathf.Lerp(orbitalTransposer.m_FollowOffset.z, targetCameraFollowZ, growthRate * Time.deltaTime);

            if (transform.localScale.x >= targetScale.x && 
                groundDistance >= targetGroundDistance && 
                targetCameraFollowZ <= orbitalTransposer.m_FollowOffset.z)
            {
                isGrowing = false;
            }
        }
    }

    public void GrowPinata()
    {
        // set initial targets
        if (targetGroundDistance == 0f)
        {
            targetScale = transform.localScale * growthFactor;
            targetGroundDistance = groundDistance * growthFactor;
            targetCameraFollowZ = orbitalTransposer.m_FollowOffset.z *= growthFactor;
        }
        // set targets using targets -- this way the target can update when multiple candies are collected simultaneously
        else
        {
            targetScale *= growthFactor;
            targetGroundDistance *= growthFactor;
            targetCameraFollowZ *= growthFactor;
        }
        isGrowing = true;
    }
}
