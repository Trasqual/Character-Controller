using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Action OnJumpPressed;
    public Action OnRollPressed;
    public Action OnMovement;

    private InputActions _playerInput;

    private void Awake()
    {
        _playerInput = new InputActions();
        _playerInput.Player.Enable();
        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        _playerInput.Player.Jump.started += JumpPressed;
        _playerInput.Player.Roll.started += RollPressed;
        _playerInput.Player.Movement.performed += MovementPressed;
    }

    public Vector3 Movement()
    {
        var movement = _playerInput.Player.Movement.ReadValue<Vector2>();

        return new Vector3(movement.x, 0f, movement.y);
    }

    private void JumpPressed(InputAction.CallbackContext ctx)
    {
        OnJumpPressed?.Invoke();
    }

    private void RollPressed(InputAction.CallbackContext ctx)
    {
        OnRollPressed?.Invoke();
    }

    private void MovementPressed(InputAction.CallbackContext ctx)
    {
        if (Movement() != Vector3.zero)
            OnMovement?.Invoke();
    }
}
