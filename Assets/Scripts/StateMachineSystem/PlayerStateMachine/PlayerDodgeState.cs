
using System.Collections.Generic;
using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem.Transitions;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem.States
{
    public class PlayerDodgeState : State, ITransition
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerInputManager _input;
        private readonly PlayerMovement _movement;
        private readonly PlayerStats _stats;
        private readonly Animator _anim;

        private Vector3 _dodgeDirection;
        private float _dodgeTimer;

        public List<Transition> Transitions { get; set; }
        private ITransition _transition;

        public PlayerDodgeState(StateMachine stateMachine) : base(stateMachine)
        {
            _playerStateMachine = stateMachine as PlayerStateMachine;
            _input = _playerStateMachine.Input;
            _movement = _playerStateMachine.Movement;
            _stats = _playerStateMachine.Stats;
            _anim = _playerStateMachine.Animator;

            _transition = this;
            Transitions = new();

            _transition.AddTransition(typeof(PlayerIdleState), () => _dodgeTimer <= 0f, () => false);
            _transition.AddTransition(typeof(PlayerMovementState), () => _dodgeTimer <= 0f, () => false);
            _transition.AddTransition(typeof(PlayerFallingState), () => _dodgeTimer <= 0f, () => false);
        }


        public override void EnterState()
        {
            _dodgeDirection = _input.Movement() == Vector3.zero ? _stateMachine.transform.forward : _input.Movement().normalized;
            _stateMachine.transform.rotation = Quaternion.LookRotation(_dodgeDirection);
            SetAnimSpeed("Dodge", "DodgeSpeedMultiplier", _stats.DodgeDuration);
            _anim.SetTrigger("Dodge");
        }

        public override void ExitState()
        {
            _dodgeDirection = Vector3.zero;
        }

        public override void UpdateState()
        {
            _dodgeTimer += Time.deltaTime;

            if (_dodgeTimer < _stats.DodgeDuration)
            {
                _movement.ApplyMovement(_dodgeDirection, _stats.DodgeCurve.Evaluate(_dodgeTimer / _stats.DodgeDuration) * _stats.MovementSpeed);
                _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity * 2f);
                _movement.Move();
            }
            else
            {
                _dodgeTimer = 0f;

                if (!_movement.IsGrounded)
                {
                    _stateMachine.ChangeState<PlayerFallingState>();
                }
                else
                {

                    if (_input.Movement().magnitude > 0)
                    {
                        _stateMachine.ChangeState<PlayerMovementState>();
                    }
                    else
                    {
                        _stateMachine.ChangeState<PlayerIdleState>();
                    }
                }
            }
        }

        public override void CancelState()
        {

        }

        private void SetAnimSpeed(string animName, string speedMultiplier, float value)
        {
            RuntimeAnimatorController ac = _anim.runtimeAnimatorController;
            var dodgeAnimTime = 0f;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name == animName)
                {
                    dodgeAnimTime = ac.animationClips[i].length;
                }
            }

            _anim.SetFloat(speedMultiplier, dodgeAnimTime / value);
        }
    }
}
