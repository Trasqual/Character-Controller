using System.Collections.Generic;
using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem.Transitions;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem.States
{
    public class PlayerFallingState : State, ITransition
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerInputManager _input;
        private readonly PlayerMovement _movement;
        private readonly PlayerStats _stats;
        private readonly Animator _anim;
        private Vector3 _initialMovement;
        private Vector3 _initialPosition;

        private ITransition _transition;
        public List<Transition> Transitions { get; private set; }

        public PlayerFallingState(StateMachine stateMachine) : base(stateMachine)
        {
            _playerStateMachine = stateMachine as PlayerStateMachine;
            _input = _playerStateMachine.Input;
            _movement = _playerStateMachine.Movement;
            _stats = _playerStateMachine.Stats;
            _anim = _playerStateMachine.Animator;

            Transitions = new();
            _transition = this;

            _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerLandingState), () => true, () => false);
        }

        public override void EnterState()
        {
            _initialPosition = _movement.transform.position;
            _initialMovement = _movement.Velocity.normalized;
            _initialMovement.y = 0f;
            _anim.SetBool("IsGrounded", false);
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            _movement.ApplyMovement(_initialMovement, _movement.LastSpeed);
            _movement.Rotate(_input.Movement(), _stats.RotationSpeed);
            _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
            _movement.Move();
            _anim.SetFloat("VerticalVelocity", _movement.Velocity.y);

            if (_movement.IsGrounded)
            {
                var fallDistance = _initialPosition.y - _movement.transform.position.y;
                if (fallDistance >= _stats.LandingDistance)
                {
                    _playerStateMachine.ChangeState<PlayerLandingState>();
                }
                else if (_input.Movement().magnitude > 0)
                {
                    _playerStateMachine.ChangeState<PlayerMovementState>();
                }
                else
                {
                    _playerStateMachine.ChangeState<PlayerIdleState>();
                }
            }
        }

        public override void CancelState()
        {

        }
    }
}
