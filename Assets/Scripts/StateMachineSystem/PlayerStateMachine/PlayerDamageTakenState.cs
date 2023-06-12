using System.Collections.Generic;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem.Transitions;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem.States
{
    public class PlayerDamageTakenState : State, ITransition
    {
        protected PlayerStateMachine _playerStateMachine;
        private readonly PlayerMovement _movement;
        private readonly PlayerStats _stats;
        private readonly Animator _anim;

        private float _damageTakenTimer;

        public List<Transition> Transitions { get; private set; }
        private readonly ITransition _transition;

        public PlayerDamageTakenState(StateMachine stateMachine) : base(stateMachine)
        {
            _playerStateMachine = stateMachine as PlayerStateMachine;
            _movement = _playerStateMachine.Movement;
            _stats = _playerStateMachine.Stats;
            _anim = _playerStateMachine.Animator;

            Transitions = new();
            _transition = this;

            _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
        }

        public override void EnterState()
        {
            _anim.SetTrigger("DamageTaken");
            SetAnimSpeed("DamageTaken", "DamageTakenMultiplier", _stats.DamageTakenDuration);
            _movement.ApplyMovement(Vector3.zero, _stats.MovementSpeed);
            _damageTakenTimer = 0f;
        }

        public override void UpdateState()
        {
            _damageTakenTimer += Time.deltaTime;

            _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
            _movement.Move();

            if (_damageTakenTimer >= _stats.DamageTakenDuration)
            {
                _playerStateMachine.ChangeState<PlayerIdleState>();
            }
        }

        public override void ExitState()
        {

        }

        public override void CancelState()
        {

        }

        private void SetAnimSpeed(string animName, string speedMultiplier, float value)
        {
            RuntimeAnimatorController ac = _anim.runtimeAnimatorController;
            var animTime = 0f;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name == animName)
                {
                    animTime = ac.animationClips[i].length;
                }
            }

            _anim.SetFloat(speedMultiplier, animTime / value);
        }
    }
}