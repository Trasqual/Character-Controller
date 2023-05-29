
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : State, ITransition
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputManager _input;
    private readonly PlayerMovement _movement;
    private readonly PlayerStats _stats;

    private Vector3 _dodgeDirection;
    private float _dodgeTimer;

    public List<Transition> Transitions { get; set; }
    private ITransition _transition;

    public PlayerDodgeState(StateMachine stateMachine) : base(stateMachine)
    {
        _playerStateMachine = stateMachine as PlayerStateMachine;
        _input = _playerStateMachine.Input;
        _movement = _playerStateMachine.Movement;
        _stats = _playerStateMachine.Stats;

        _transition = this;
        Transitions = new();

        _transition.AddTransition(typeof(PlayerIdleState), () => _dodgeTimer <= 0, () => false);
        _transition.AddTransition(typeof(PlayerMovementState), () => _dodgeTimer <= 0, () => false);
    }


    public override void EnterState()
    {
        _dodgeDirection = _input.Movement() == Vector3.zero ? _stateMachine.transform.forward : _input.Movement();
        _stateMachine.transform.rotation = Quaternion.LookRotation(_dodgeDirection);
    }

    public override void ExitState()
    {
        _dodgeDirection = Vector3.zero;
    }

    public override void UpdateState()
    {
        _dodgeTimer += Time.deltaTime;

        if (_dodgeTimer < _stats.DodgeDuration)
        {
            _movement.Move(_dodgeDirection, _stats.DodgeCurve.Evaluate(_dodgeTimer / _stats.DodgeDuration) * _stats.DodgeSpeed);
        }
        else
        {
            _dodgeTimer = 0f;
            _stateMachine.ChangeState<PlayerMovementState>();
        }
    }

    public override void CancelState()
    {

    }
}
