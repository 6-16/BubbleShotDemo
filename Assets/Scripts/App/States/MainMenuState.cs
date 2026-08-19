using System;
using UnityEngine;

public class MainMenuState : IAppState
{
    private readonly ISceneLoader _sceneLoader;
    private readonly SceneCatalog _sceneCatalog;

    public MainMenuState(ISceneLoader sceneLoader, SceneCatalog sceneCatalog)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
    }

    public Awaitable EnterAsync()
    {
        return _sceneLoader.LoadAsync(_sceneCatalog.Menu);
    }

    public void Exit()
    {
    }
}
