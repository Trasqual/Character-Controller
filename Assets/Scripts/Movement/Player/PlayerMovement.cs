using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MovementBase
{
    [SerializeField] private Vector3 _groundedGravity = new Vector3(0f, -0.1f, 0f);
    [SerializeField] private Vector3 _notGroundedGravity = new Vector3(0f, -9.81f, 0f);

    private CharacterController _controller;
    public Vector3 Velocity => _controller.velocity;
    public bool IsGrounded => _controller.isGrounded;

    public Vector3 Gravity
    {
        get
        {
            return IsGrounded ? _groundedGravity : _notGroundedGravity;
        }
    }

    private Vector3 _movementVector;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void SetMoveVelocity(Vector3 movementVector, float speed)
    {
        _movementVector = speed * Time.deltaTime * movementVector;
    }

    public override void Move()
    {
        _controller.Move(_movementVector);
    }

    public void Rotate(Vector3 rotationVector, float speed)
    {
        if (rotationVector != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationVector), speed * Time.deltaTime);
    }

    public void ApplyJumpVelocity(Vector3 vertVelocity)
    {
        _movementVector += vertVelocity;
    }

    public void ApplyGravity()
    {
        if (IsGrounded && _movementVector.y < 0)
        {
            _movementVector.y = _groundedGravity.y;
        }
        else
        {
            _movementVector += _notGroundedGravity;
        }
    }
}
