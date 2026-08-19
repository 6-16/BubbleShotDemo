using System;
using UnityEngine;

public interface IShotInput
{
    event Action Began;
    event Action<Vector2> Aimed;
    event Action Released;

    bool IsHeld { get; }
}
