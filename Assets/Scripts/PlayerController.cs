using System.Collections;
using System.Collections.Generic;
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


    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        // Rotate character
        _playerTargetRotation.x += transform.eulerAngles.x + rotationSpeed * _playerLocomotionInput.RotateInput.x * angularDrag * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        if (!jumpReleased && _playerLocomotionInput.GetJump < 0.1)
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
        if (_playerLocomotionInput.GetJump > 0 && isGrounded && jumpReleased)
        {
            verticalVelocity += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Reduce upward velocity when the jump button is released early
        else if (_playerLocomotionInput.GetJump == 0)
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
    }

    private void LateUpdate()
    {

        _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
    }
}
