using System;
using UnityEngine;

public interface ISceneLoader
{
    event Action<float> ProgressChanged;

    float Progress { get; }

    Awaitable LoadAsync(SceneReference target);
}
