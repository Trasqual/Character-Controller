
public class PlayerCharacter : CharacterBase
{
    private PlayerInputManager _input;

    protected override void Awake()
    {
        base.Awake();

        _input = GetComponent<PlayerInputManager>();
        _stateMachine = _stateMachine as PlayerStateMachine;

        _stateMachine.AddState(new PlayerIdleState(_stateMachine));
        _stateMachine.AddState(new PlayerMovementState(_stateMachine));
        _stateMachine.AddState(new PlayerDodgeState(_stateMachine));
        _stateMachine.ChangeState<PlayerIdleState>();

        _input.OnRollPressed += Dodge;
        _input.OnJumpPressed += Jump;
        _input.OnMovement += Move;
    }

    private void Move()
    {
        _stateMachine.ChangeState<PlayerMovementState>();
    }

    private void Dodge()
    {
        _stateMachine.ChangeState<PlayerDodgeState>();
    }

    private void Jump()
    {

    }

}
