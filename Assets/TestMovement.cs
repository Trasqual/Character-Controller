using UnityEngine;

public class TestMovement : MonoBehaviour
{
    CharacterController _controller;
    PlayerInputManager _input;
    Vector3 _movementVector;
    bool _shouldJump;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputManager>();
        _input.OnJumpPressed += Jump;
    }

    private void Update()
    {
        ApplyMovement();
        ApplyJump();
        ApplyGravity();

        _controller.Move(_movementVector * Time.deltaTime);
    }

    private void ApplyMovement()
    {
         var speed = _controller.isGrounded ? 8f : 4f;
            _movementVector = new Vector3(_input.Movement().x * speed, _movementVector.y, _input.Movement().z * speed);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _movementVector.y < 0f)
        {
            _movementVector.y = -0.1f;
        }
        else
        {
            _movementVector.y += -20f * Time.deltaTime;
        }
    }

    private void ApplyJump()
    {
        if (_controller.isGrounded && _shouldJump)
        {
            _movementVector *= 0.5f;
            _movementVector.y = 15f;
            _shouldJump = false;
        }
    }

    private void Jump()
    {
        _shouldJump = true;
    }
}
