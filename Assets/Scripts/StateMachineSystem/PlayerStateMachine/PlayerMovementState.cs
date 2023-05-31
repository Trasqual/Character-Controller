
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputManager _input;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;

    private ITransition _transition;
    public List<Transition> Transitions { get; private set; }

    public PlayerMovementState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _input = _playerStateMachine.Input;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerDodgeState), () => true, () => true);
        _transition.AddTransition(typeof(PlayerJumpingState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerFallingState), () => true, () => false);
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        _movement.SetMoveVelocity(_input.Movement(), _stats.MovementSpeed);
        _movement.ApplyGravity();
        _movement.Rotate(_input.Movement(), _stats.RotationSpeed);
        _movement.Move();

        if (_input.Movement() == Vector3.zero)
        {
            _playerStateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void CancelState()
    {

    }
}
