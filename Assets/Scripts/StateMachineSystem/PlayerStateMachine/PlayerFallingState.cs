using System.Collections.Generic;

public class PlayerFallingState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputManager _input;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;

    private ITransition _transition;
    public List<Transition> Transitions { get; private set; }

    public PlayerFallingState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _input = _playerStateMachine.Input;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerIdleState), () => true, () => false);
        _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        _movement.ApplyMovement(_input.Movement(), _stats.MovementSpeed * _stats.OnAirMovementSpeedModifier);
        _movement.Rotate(_input.Movement(), _stats.RotationSpeed);
        _movement.ApplyGravity(_stats.GroundedGravity,_stats.OnAirGravity);
        _movement.Move();

        if (_movement.IsGrounded)
        {
            _playerStateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void CancelState()
    {

    }
}
