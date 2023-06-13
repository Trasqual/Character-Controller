using UnityEngine;

namespace Scripts.MovementSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MovementBase
    {
        [SerializeField] private LayerMask _groundLayer;

        private CharacterController _controller;

        public Vector3 Velocity => _controller.velocity;
        public bool IsGrounded => _controller.isGrounded;
        public float LastSpeed { get; private set; }

        private Vector3 _slopeHitNormal;

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

        public void ApplySlide(float slopeSpeed, Vector3 userInput)
        {
            if (ShouldSlide)
            {
                var slideVector = new Vector3(_slopeHitNormal.x, -_slopeHitNormal.y, _slopeHitNormal.z);
                var tempSlopeSpeed = slopeSpeed;
                if (Vector3.Dot(userInput.normalized, slideVector) < 0) tempSlopeSpeed *= 3.25f;

                //var angleFactor = 1f - Mathf.InverseLerp(0, 90, Vector3.Angle(_slopeHitNormal, Vector3.up));
                _movementVector += tempSlopeSpeed * slideVector;
            }
        }

        public bool ShouldSlide
        {
            get
            {
                if (_controller.isGrounded && Physics.SphereCast(transform.position + Vector3.up * 0.5f, _controller.radius * 0.95f, Vector3.down, out RaycastHit hit, 1.5f, _groundLayer))
                {
                    _slopeHitNormal = hit.normal;
                    return Vector3.Angle(_slopeHitNormal, Vector3.up) > _controller.slopeLimit;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
