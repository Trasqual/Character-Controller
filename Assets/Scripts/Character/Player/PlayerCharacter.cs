
using DG.Tweening;

public class PlayerCharacter : CharacterBase
{
    private PlayerInputManager _input;
    private PlayerStats _stats;
    private GravityHandler _gravityHandler;

    private bool _canDodge = true;
    private Tween _dodgeCooldown;

    protected override void Awake()
    {
        base.Awake();

        _input = GetComponent<PlayerInputManager>();
        _stateMachine = _stateMachine as PlayerStateMachine;
        _stats = GetComponent<PlayerStats>();
        _gravityHandler = GetComponentInChildren<GravityHandler>();

        _stateMachine.AddState(new PlayerIdleState(_stateMachine));
        _stateMachine.AddState(new PlayerMovementState(_stateMachine));
        _stateMachine.AddState(new PlayerDodgeState(_stateMachine));
        _stateMachine.AddState(new PlayerJumpingState(_stateMachine));
        _stateMachine.AddState(new PlayerFallingState(_stateMachine));
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
        if (_canDodge)
        {
            _stateMachine.ChangeState<PlayerDodgeState>();
            _canDodge = false;
            _dodgeCooldown = DOVirtual.DelayedCall(_stats.DodgeCooldown, () => _canDodge = true);
        }
    }

    private void Jump()
    {
        if (_gravityHandler.IsGrounded)
        {
            _stateMachine.ChangeState<PlayerJumpingState>();
        }
    }
}
