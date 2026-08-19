using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private const float ActivationProgressThreshold = 0.9f;

    private readonly SceneCatalog _sceneCatalog;

    private float _progress;

    public event Action<float> ProgressChanged;

    public float Progress => _progress;

    public SceneLoader(SceneCatalog sceneCatalog)
    {
        _sceneCatalog = sceneCatalog != null ? sceneCatalog : throw new ArgumentNullException(nameof(sceneCatalog));
    }

    public async Awaitable LoadAsync(SceneReference target)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (!target.IsAssigned) throw new InvalidOperationException($"{nameof(SceneCatalog)} has an unassigned scene.");
        if (IsActive(target)) return;

        SetProgress(0f);

        if (!IsActive(_sceneCatalog.Loading))
        {
            await LoadSceneAsync(_sceneCatalog.Loading);
        }

        await LoadTrackedAsync(target);
    }

    private async Awaitable LoadTrackedAsync(SceneReference target)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(target.SceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < ActivationProgressThreshold)
        {
            SetProgress(operation.progress / ActivationProgressThreshold);

            await Awaitable.NextFrameAsync();
        }

        SetProgress(1f);

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            await Awaitable.NextFrameAsync();
        }
    }

    private async Awaitable LoadSceneAsync(SceneReference scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.SceneName);

        while (!operation.isDone)
        {
            await Awaitable.NextFrameAsync();
        }
    }

    private void SetProgress(float value)
    {
        if (Mathf.Approximately(_progress, value)) return;

        _progress = value;
        ProgressChanged?.Invoke(value);
    }

    private bool IsActive(SceneReference scene)
    {
        return SceneManager.GetActiveScene().name == scene.SceneName;
    }
}
