using System.Collections.Generic;
using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem.Transitions;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem.States
{
    public class PlayerJumpAttackState : State, ITransition
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerInputManager _input;
        private readonly PlayerMovement _movement;
        private readonly PlayerStats _stats;
        private readonly Animator _anim;

        private readonly ITransition _transition;
        public List<Transition> Transitions { get; private set; }


        public PlayerJumpAttackState(StateMachine stateMachine) : base(stateMachine)
        {
            _playerStateMachine = stateMachine as PlayerStateMachine;
            _input = _playerStateMachine.Input;
            _movement = _playerStateMachine.Movement;
            _stats = _playerStateMachine.Stats;
            _anim = _playerStateMachine.Animator;

            Transitions = new();
            _transition = this;

            _transition.AddTransition(typeof(PlayerFallingState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
            _transition.AddTransition(typeof(PlayerDodgeState), () => true, () => true);
        }

        public override void EnterState()
        {
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            var airSpeed = _stats.MovementSpeed * _stats.OnAirMovementSpeedModifier;
            if (_input.Movement() != Vector3.zero)
                _movement.ApplyMovement(_input.Movement(), airSpeed);
            _movement.Move();

            _anim.SetFloat("VerticalVelocity", _movement.Velocity.y);

            if (_movement.Velocity.y <= 0)
            {
                _playerStateMachine.ChangeState<PlayerFallingState>();
            }
        }

        public override void CancelState()
        {

        }
    }
}

