using System.Collections.Generic;
using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem.Transitions;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem.States
{
    public class PlayerMovementState : State, ITransition
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerInputManager _input;
        private readonly PlayerMovement _movement;
        private readonly PlayerStats _stats;
        private readonly Animator _anim;

        private ITransition _transition;
        public List<Transition> Transitions { get; private set; }

        public PlayerMovementState(StateMachine stateMachine) : base(stateMachine)
        {
            _playerStateMachine = stateMachine as PlayerStateMachine;
            _input = _playerStateMachine.Input;
            _movement = _playerStateMachine.Movement;
            _stats = _playerStateMachine.Stats;
            _anim = _playerStateMachine.Animator;

            Transitions = new();
            _transition = this;

            _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerDodgeState), () => true, () => true);
            _transition.AddTransition(typeof(PlayerJumpingState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerFallingState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerDamageTakenState), () => true, () => true);
        }

        public override void EnterState()
        {
            _anim.SetBool("IsGrounded", true);
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            _anim.SetFloat("Movement", Vector3.Dot(_input.Movement(), _stateMachine.transform.forward), 0.1f, Time.deltaTime);
            _movement.ApplyMovement(_input.Movement(), _stats.MovementSpeed);
            _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
            _movement.ApplySlide(_stats.SlopeSlideSpeed);
            _movement.Rotate(_input.Movement(), _stats.RotationSpeed);
            _movement.Move();

            if (_input.Movement() == Vector3.zero)
            {
                _playerStateMachine.ChangeState<PlayerIdleState>();
            }
            if (_movement.Velocity.y < _stats.GroundedGravity && !_movement.IsGrounded)
            {
                _playerStateMachine.ChangeState<PlayerFallingState>();
            }
        }

        public override void CancelState()
        {

        }
    }
}
