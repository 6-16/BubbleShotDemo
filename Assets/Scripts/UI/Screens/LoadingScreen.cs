using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadingScreen : UiScreen
{
    [SerializeField] private Image _progressFill;

    private ISceneLoader _sceneLoader;
    private bool _isSubscribed;

    [Inject]
    private void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    protected override void OnOpening()
    {
        Subscribe();
        OnProgressChanged(_sceneLoader.Progress);
    }

    protected override void OnClosing()
    {
        Unsubscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_isSubscribed) return;

        _sceneLoader.ProgressChanged += OnProgressChanged;
        _isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (!_isSubscribed) return;

        _sceneLoader.ProgressChanged -= OnProgressChanged;
        _isSubscribed = false;
    }

    private void OnProgressChanged(float progress)
    {
        _progressFill.fillAmount = progress;
    }
}
