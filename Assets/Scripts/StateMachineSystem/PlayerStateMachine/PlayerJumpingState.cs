using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputManager _input;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;
    private bool _shouldJump;

    private readonly ITransition _transition;
    public List<Transition> Transitions { get; private set; }


    public PlayerJumpingState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _input = _playerStateMachine.Input;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerFallingState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
    }

    public override void EnterState()
    {
        _movement.ApplyJump(_stats.JumpPower);
        _shouldJump = true;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        _movement.ApplyMovement(_input.Movement(), _stats.MovementSpeed * _stats.OnAirMovementSpeedModifier);
        _movement.Rotate(_input.Movement(), _stats.RotationSpeed);
        if (!_shouldJump)
        {
            _movement.ApplyGravity(_stats.GroundedGravity, _stats.OnAirGravity);
        }
        _shouldJump = false;
        _movement.Move();
        if (_movement.Velocity.y <= 0)
        {
            _playerStateMachine.ChangeState<PlayerFallingState>();
        }
    }

    public override void CancelState()
    {

    }
}
