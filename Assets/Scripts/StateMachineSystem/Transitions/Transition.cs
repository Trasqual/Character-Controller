using System;

namespace Scripts.StateMachineSystem.Transitions
{
    public class Transition
    {
        public Type To;
        public Func<bool> Condition;
        public Func<bool> Override;

        public Transition(Type to, Func<bool> condition, Func<bool> shouldOverride)
        {
            To = to;
            Condition = condition;
            Override = shouldOverride;
        }
    }
}

