using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResultScreen : UiScreenWithArgs<ResultArgs>
{
    [SerializeField] private GameObject _winRoot;
    [SerializeField] private GameObject _loseRoot;
    [SerializeField] private Button _menuButton;

    private AppStateMachine _stateMachine;

    [Inject]
    private void Construct(AppStateMachine stateMachine)
    {
        _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
    }

    protected override void OnArgsReceived()
    {
        _winRoot.SetActive(Args.IsWin);
        _loseRoot.SetActive(!Args.IsWin);
    }

    protected override void OnOpened()
    {
        _menuButton.onClick.AddListener(OnMenuClicked);
    }

    protected override void OnClosing()
    {
        _menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    private async void OnMenuClicked()
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
