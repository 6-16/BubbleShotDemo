using System;
using Zenject;

public class GameplayResult : IInitializable, IDisposable
{
    private readonly PathTraversal _traversal;
    private readonly PlayerSize _playerSize;

    private bool _isDecided;

    public event Action<bool> Finished;

    public bool IsDecided => _isDecided;

    public GameplayResult(PathTraversal traversal, PlayerSize playerSize)
    {
        _traversal = traversal ?? throw new ArgumentNullException(nameof(traversal));
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));
    }

    public void Initialize()
    {
        _traversal.Finished += OnTraversalFinished;
        _playerSize.Changed += OnPlayerSizeChanged;
    }

    public void Dispose()
    {
        _traversal.Finished -= OnTraversalFinished;
        _playerSize.Changed -= OnPlayerSizeChanged;
    }

    private void OnTraversalFinished()
    {
        Decide(true);
    }

    private void OnPlayerSizeChanged()
    {
        if (!_playerSize.IsDepleted) return;

        Decide(false);
    }

    private void Decide(bool isWin)
    {
        if (_isDecided) return;

        _isDecided = true;

        Finished?.Invoke(isWin);
    }
}
