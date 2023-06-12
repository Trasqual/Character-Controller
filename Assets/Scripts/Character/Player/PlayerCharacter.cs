using Scripts.HealthSystem;
using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StateMachineSystem;
using Scripts.StateMachineSystem.States;

namespace Scripts.Characters
{
    public class PlayerCharacter : CharacterBase, IDamagable
    {
        private PlayerInputManager _input;
        private PlayerMovement _movement;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInputManager>();
            _stateMachine = _stateMachine as PlayerStateMachine;
            _movement = GetComponent<PlayerMovement>();

            _stateMachine.AddState(new PlayerIdleState(_stateMachine));
            _stateMachine.AddState(new PlayerMovementState(_stateMachine));
            _stateMachine.AddState(new PlayerDodgeState(_stateMachine));
            _stateMachine.AddState(new PlayerJumpingState(_stateMachine));
            _stateMachine.AddState(new PlayerFallingState(_stateMachine));
            _stateMachine.AddState(new PlayerLandingState(_stateMachine));
            _stateMachine.AddState(new PlayerDamageTakenState(_stateMachine));

            _stateMachine.ChangeState<PlayerIdleState>();

            _input.OnRollPressed += Dodge;
            _input.OnJumpPressed += Jump;
        }

        private void Dodge()
        {
            _stateMachine.ChangeState<PlayerDodgeState>();
        }

        private void Jump()
        {
            if (_movement.IsGrounded)
            {
                _stateMachine.ChangeState<PlayerJumpingState>();
            }
        }

        public void TakeDamage(float damage)
        {
            _stateMachine.ChangeState<PlayerDamageTakenState>();
        }
    }
}
