using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHudScreen : UiScreen
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Image _volumeFill;

    private AppStateMachine _stateMachine;
    private PlayerSize _playerSize;
    private bool _isSubscribed;

    [Inject]
    private void Construct(AppStateMachine stateMachine, PlayerSize playerSize)
    {
        _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));
    }

    protected override void OnOpening()
    {
        Subscribe();
        OnPlayerSizeChanged();
    }

    protected override void OnOpened()
    {
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    protected override void OnClosing()
    {
        _exitButton.onClick.RemoveListener(OnExitClicked);

        Unsubscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_isSubscribed) return;

        _playerSize.Changed += OnPlayerSizeChanged;
        _isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (!_isSubscribed) return;

        _playerSize.Changed -= OnPlayerSizeChanged;
        _isSubscribed = false;
    }

    private void OnPlayerSizeChanged()
    {
        _volumeFill.fillAmount = _playerSize.NormalizedAvailable;
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
