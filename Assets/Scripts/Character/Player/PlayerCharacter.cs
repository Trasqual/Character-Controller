
using DG.Tweening;

public class PlayerCharacter : CharacterBase
{
    private PlayerInputManager _input;
    private PlayerStats _stats;
    private PlayerMovement _movement;

    private bool _canDodge = true;
    private Tween _dodgeCooldownTween;

    protected override void Awake()
    {
        base.Awake();

        _input = GetComponent<PlayerInputManager>();
        _stateMachine = _stateMachine as PlayerStateMachine;
        _stats = GetComponent<PlayerStats>();
        _movement = GetComponent<PlayerMovement>();

        _stateMachine.AddState(new PlayerIdleState(_stateMachine));
        _stateMachine.AddState(new PlayerMovementState(_stateMachine));
        _stateMachine.AddState(new PlayerDodgeState(_stateMachine));
        _stateMachine.AddState(new PlayerJumpingState(_stateMachine));
        _stateMachine.AddState(new PlayerFallingState(_stateMachine));
        _stateMachine.ChangeState<PlayerIdleState>();

        _input.OnRollPressed += Dodge;
        _input.OnJumpPressed += Jump;
    }

    private void Dodge()
    {
        if (_canDodge)
        {
            _stateMachine.ChangeState<PlayerDodgeState>();
            _canDodge = false;
            _dodgeCooldownTween = DOVirtual.DelayedCall(_stats.DodgeCooldown, () => _canDodge = true);
        }
    }

    private void Jump()
    {
        if (_movement.IsGrounded)
        {
            _stateMachine.ChangeState<PlayerJumpingState>();
        }
    }
}
