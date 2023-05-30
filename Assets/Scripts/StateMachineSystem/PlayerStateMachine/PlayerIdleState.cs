
using System.Collections.Generic;

public class PlayerIdleState : State, ITransition
{
    protected PlayerStateMachine _playerStateMachine;

    public List<Transition> Transitions { get; private set; }
    private ITransition _transition;

    public PlayerIdleState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;

        Transitions = new();
        _transition = this;

        _transition.AddTransition(typeof(PlayerMovementState), () => true, () => false);
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
        _playerStateMachine.Movement.ApplyGravity();
    }

    public override void CancelState()
    {

    }
}
