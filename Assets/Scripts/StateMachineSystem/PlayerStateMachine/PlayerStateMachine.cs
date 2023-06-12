using Scripts.InputSystem;
using Scripts.MovementSystem;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.StateMachineSystem
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public PlayerMovement Movement { get; private set; }
        [field: SerializeField] public PlayerInputManager Input { get; private set; }
        [field: SerializeField] public PlayerStats Stats { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
    }
}
