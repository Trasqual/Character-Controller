
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State, ITransition
{
    protected PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputManager _input;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;
    private readonly Animator _anim;

    public List<Transition> Transitions { get; private set; }
    private readonly ITransition _transition;

    public PlayerIdleState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _input = _playerStateMachine.Input;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;
        _anim = _playerStateMachine.Animator;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerDodgeState), () => true, () => true);
        _transition.AddTransition(typeof(PlayerJumpingState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerFallingState), () => true, () => false);
    }


    public override void EnterState()
    {
        _anim.SetFloat("Movement", 0f);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        _movement.ApplyMovement(_input.Movement(), _stats.MovementSpeed);
        _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
        _movement.Move();

        if (_input.Movement().magnitude > 0)
            _playerStateMachine.ChangeState<PlayerMovementState>();
    }

    public override void CancelState()
    {

    }
}
