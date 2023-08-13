using System.Collections.Generic;
using System.Linq;
using Scripts.StateMachineSystem.States;
using Scripts.StateMachineSystem.Transitions;
using UnityEngine;

namespace Scripts.StateMachineSystem
{
    public class StateMachine : MonoBehaviour
    {
        private readonly List<State> _states = new();

        private State _currentState;

        public void AddState(State state)
        {
            if (!_states.Contains(state))
                _states.Add(state);
        }

        public void RemoveState<T>() where T : State
        {
            var state = GetState<T>();

            if (state != null)
            {
                _states.Remove(state);
            }
        }

        public void ChangeState<T>() where T : State
        {
            var state = GetState<T>();

            if (_currentState != null && _currentState == state)
            {
                return;
            }

            if (_currentState == null && state != null)
            {
                _currentState = GetState<T>();
                _currentState.EnterState();
                return;
            }

            if (state != null)
            {
                if (_currentState is ITransition transition)
                {
                    if (transition.TryGetTransition(state.GetType(), out var suitableTransition))
                    {
                        if (suitableTransition.Condition())
                        {
                            if (suitableTransition.Override())
                            {
                                _currentState.CancelState();
                            }
                            else
                            {
                                _currentState.ExitState();
                            }


                            _currentState = state;
                            _currentState.EnterState();

                            //Debug.LogWarning(_currentState.GetType().ToString());
                        }
                    }
                }
            }
        }

        private State GetState<T>() where T : State
        {
            return _states.OfType<T>().FirstOrDefault();
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }
    }
}
