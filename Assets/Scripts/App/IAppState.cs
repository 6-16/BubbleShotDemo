using UnityEngine;

public interface IAppState
{
    Awaitable EnterAsync();
    void Exit();
}
