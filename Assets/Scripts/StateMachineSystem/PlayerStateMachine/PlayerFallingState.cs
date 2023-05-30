using System.Collections.Generic;

public class PlayerFallingState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerMovement _movement;

    private ITransition _transition;
    public List<Transition> Transitions { get; private set; }

    public PlayerFallingState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _movement = _playerStateMachine.Movement;

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
        if (_movement.IsGrounded)
        {
            _playerStateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void CancelState()
    {

    }
}
