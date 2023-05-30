using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MovementBase
{
    [SerializeField] private GravityHandler _gravity;

    private CharacterController _controller;

    public Vector3 Velocity => _controller.velocity;
    public bool IsGrounded => _controller.isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = GetComponentInChildren<GravityHandler>();
    }

    public override void Move(Vector3 movementVector, float speed)
    {
        _controller.Move(Time.deltaTime * new Vector3(movementVector.x * speed, movementVector.y, movementVector.z * speed));
    }

    public void Rotate(Vector3 rotationVector, float speed)
    {
        if (rotationVector != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationVector), speed * Time.deltaTime);
    }

    public void ApplyGravity()
    {
        _controller.Move(_gravity.Gravity * Time.deltaTime);
    }
}
