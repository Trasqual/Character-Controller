using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Action OnJumpPressed;
    public Action OnRollPressed;
    public Action OnMovement;

    [field: SerializeField] public bool UseMouseMovement { get; private set; }

    private InputActions _playerInput;
    private Camera _cam;

    private bool _isRightClickPressed;

    private void Awake()
    {
        _cam = Camera.main;
        _playerInput = new InputActions();
        _playerInput.Player.Enable();
        SubscribeToInputs();
    }

    private void SubscribeToInputs()
    {
        _playerInput.Player.Jump.started += JumpPressed;
        _playerInput.Player.Roll.started += RollPressed;
        _playerInput.Player.Movement.performed += MovementPressed;
        _playerInput.Player.RightClick.started += RightClickPressed;
        _playerInput.Player.RightClick.canceled += RightClickCanceled;
    }

    public Vector3 Movement()
    {
        var movement = _playerInput.Player.Movement.ReadValue<Vector2>();

        bool shouldUseLookInput = UseMouseMovement && _isRightClickPressed;

        return shouldUseLookInput ? Look() : new Vector3(movement.x, 0f, movement.y);
    }

    public Vector3 Look()
    {
        var readLookVector = _playerInput.Player.Look.ReadValue<Vector2>();
        var look = Vector3.zero;
        if (_playerInput.Player.Look.activeControl != null)
        {
            if (_playerInput.Player.Look.activeControl.device.displayName == "Mouse")
            {
                look = GetAxisFromMousePos(readLookVector);
            }
            else
            {
                look = new Vector3(readLookVector.x, 0f, readLookVector.y);
            }
        }

        return look;
    }

    private void RightClickPressed(InputAction.CallbackContext ctx)
    {
        _isRightClickPressed = true;
    }

    private void RightClickCanceled(InputAction.CallbackContext ctx)
    {
        _isRightClickPressed = false;
    }

    private Vector3 GetAxisFromMousePos(Vector2 mousePosition)
    {
        var playerPos = _cam.WorldToScreenPoint(transform.position);
        playerPos.z = 0f;
        var maxMag = 350f;
        var dir = (Vector3)mousePosition - playerPos;
        var magFactor = Mathf.Clamp(dir.magnitude, 0f, maxMag) / maxMag;
        dir.Normalize();

        if (UseMouseMovement)
            dir = dir * magFactor;

        dir.z = dir.y;
        dir.y = 0f;
        dir = Quaternion.Euler(0f, _cam.transform.rotation.y, 0f) * dir;
        return dir;
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
