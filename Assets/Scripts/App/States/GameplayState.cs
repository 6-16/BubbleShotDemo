using System;
using UnityEngine;

public class GameplayState : IAppState
{
    private readonly ISceneLoader _sceneLoader;
    private readonly SceneCatalog _sceneCatalog;

    public GameplayState(ISceneLoader sceneLoader, SceneCatalog sceneCatalog)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
    }

    public Awaitable EnterAsync()
    {
        return _sceneLoader.LoadAsync(_sceneCatalog.Gameplay);
    }

    public void Exit()
    {
    }
}
