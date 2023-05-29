using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public PlayerMovement Movement { get; private set; }
    [field: SerializeField] public PlayerInputManager Input { get; private set; }
    [field: SerializeField] public PlayerStats Stats { get; private set; }
}
