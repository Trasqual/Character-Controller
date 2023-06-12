using Scripts.StateMachineSystem;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.Characters
{
    [RequireComponent(typeof(PlayerStateMachine))]
    public class CharacterBase : MonoBehaviour
    {
        [field: SerializeField] public CharacterStats Stats { get; private set; }

        protected StateMachine _stateMachine;

        protected virtual void Awake()
        {
            _stateMachine = GetComponent<PlayerStateMachine>();
        }
    }
}
