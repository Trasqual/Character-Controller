using System;
using System.Collections.Generic;
using System.Linq;

public interface ITransition
{
    public List<Transition> Transitions { get; }

    public Transition GetTransition(Type to)
    {
        return Transitions.FirstOrDefault(elem => elem.To == to);
    }

    public void AddTransition(Type to, Func<bool> condition, Func<bool> shouldOverride)
    {
        Transitions.Add(new Transition(to, condition, shouldOverride));
    }

    public void RemoveTransition(Type to)
    {
        var targetTransition = Transitions.FirstOrDefault(elem => elem.To == to);
        Transitions.Remove(targetTransition);
    }

    public bool TryGetTransition(Type to, out Transition targetTransition)
    {
        foreach (var transition in Transitions)
        {
            if (transition.To == to)
            {
                targetTransition = transition;
                return true;
            }
        }

        targetTransition = null;
        return false;
    }
}