using System;
using System.Collections.Generic;
using UnityEngine;

public class AppStateMachine
{
    private readonly Dictionary<Type, IAppState> _states = new Dictionary<Type, IAppState>();

    private IAppState _currentState;

    public AppStateMachine(List<IAppState> states)
    {
        if (states == null) throw new ArgumentNullException(nameof(states));

        foreach (IAppState state in states)
        {
            _states.Add(state.GetType(), state);
        }
    }

    public async Awaitable EnterAsync<TState>() where TState : IAppState
    {
        Type stateType = typeof(TState);

        if (!_states.TryGetValue(stateType, out IAppState next))
        {
            throw new InvalidOperationException(
                $"No app state bound for '{stateType.Name}'. Bind it in {nameof(ProjectInstaller)}.");
        }

        if (ReferenceEquals(_currentState, next)) return;

        _currentState?.Exit();
        _currentState = next;

        await next.EnterAsync();
    }
}
