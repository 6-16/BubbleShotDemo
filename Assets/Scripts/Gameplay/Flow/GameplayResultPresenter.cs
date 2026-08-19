using System;
using UnityEngine;
using Zenject;

public class GameplayResultPresenter : IInitializable, IDisposable
{
    private readonly GameplayResult _result;
    private readonly IUiService _uiService;

    public GameplayResultPresenter(GameplayResult result, IUiService uiService)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
    }

    public void Initialize()
    {
        _result.Finished += OnResultFinished;
    }

    public void Dispose()
    {
        _result.Finished -= OnResultFinished;
    }

    private async void OnResultFinished(bool isWin)
    {
        try
        {
            await _uiService.ShowAsync<ResultScreen, ResultArgs>(new ResultArgs(isWin));
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
