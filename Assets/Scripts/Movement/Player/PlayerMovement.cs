using UnityEngine;

namespace Scripts.MovementSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MovementBase
    {
        [SerializeField] private Vector3 _groundedGravity = new Vector3(0f, -0.1f, 0f);
        [SerializeField] private Vector3 _notGroundedGravity = new Vector3(0f, -9.81f, 0f);

        private CharacterController _controller;
        public Vector3 Velocity => _controller.velocity;
        public bool IsGrounded => _controller.isGrounded;
        public float LastSpeed { get; private set; }

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

        public override void Move()
        {
            _controller.Move(_movementVector * Time.deltaTime);
        }

        public void Rotate(Vector3 rotationVector, float speed)
        {
            if (rotationVector != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationVector), speed * Time.deltaTime);
        }

        public void ApplyMovement(Vector3 movement, float speed)
        {
            LastSpeed = speed;
            _movementVector = new Vector3(movement.x * speed, _movementVector.y, movement.z * speed);
        }

        public void ApplyGravity(float groundedGravity, float onAirGravity)
        {
            if (_controller.isGrounded && _movementVector.y < 0f)
            {
                _movementVector.y = groundedGravity;
            }
            else
            {
                _movementVector.y += onAirGravity * Time.deltaTime;
            }
        }

        public void ApplyJump(float jumpPower)
        {
            if (_controller.isGrounded)
            {
                _movementVector.y = jumpPower;
            }
        }
    }
}
