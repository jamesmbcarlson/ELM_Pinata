using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{   
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 RotateInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public InputAction GetJump {  get; private set; }
    public InputAction GetAttack { get; private set; }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);

        GetJump = PlayerControls.PlayerLocomotionMap.Jump;
        GetAttack = PlayerControls.PlayerLocomotionMap.Attack;
    }

    private void OnDisable()
    {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        //print(MovementInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        RotateInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        GetJump.ReadValue<float>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        GetAttack.ReadValue<float>();
    }
}
