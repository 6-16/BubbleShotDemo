using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHudScreen : UiScreen
{
    [SerializeField] private Button _exitButton;

    private AppStateMachine _stateMachine;

    [Inject]
    private void Construct(AppStateMachine stateMachine)
    {
        _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
    }

    protected override void OnOpened()
    {
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    protected override void OnClosing()
    {
        _exitButton.onClick.RemoveListener(OnExitClicked);
    }

    private async void OnExitClicked()
    {
        try
        {
            await _stateMachine.EnterAsync<MainMenuState>();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
