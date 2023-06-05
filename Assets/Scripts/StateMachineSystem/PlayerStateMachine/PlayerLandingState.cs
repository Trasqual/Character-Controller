using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;
    private readonly Animator _anim;

    public List<Transition> Transitions { get; private set; }
    private readonly ITransition _transition;

    private Tween _landingDurationTween;

    public PlayerLandingState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;
        _anim = _playerStateMachine.Animator;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerDodgeState), () => true, () => true);
    }

    public override void CancelState()
    {
        _landingDurationTween?.Kill();
    }

    public override void EnterState()
    {
        _movement.ApplyMovement(Vector3.zero, 0f);
        _anim.SetTrigger("Landing");
        _landingDurationTween = DOVirtual.DelayedCall(_stats.LandingDuration, () => _playerStateMachine.ChangeState<PlayerIdleState>());
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
        _movement.Move();
    }
}
