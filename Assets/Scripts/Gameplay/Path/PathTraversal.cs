using System;
using UnityEngine;
using Zenject;

public class PathTraversal : IInitializable, ITickable, IDisposable
{
    private readonly PathSpline _spline;
    private readonly PathClearance _clearance;
    private readonly PlayerRoot _player;
    private readonly PlayerSize _playerSize;
    private readonly PlayerConfig _config;

    private float _travelledDistance;
    private bool _isTraversing;

    public event Action Finished;

    public bool IsTraversing => _isTraversing;

    public PathTraversal(
        PathSpline spline,
        PathClearance clearance,
        PlayerRoot player,
        PlayerSize playerSize,
        PlayerConfig config)
    {
        _spline = spline != null ? spline : throw new ArgumentNullException(nameof(spline));
        _clearance = clearance ?? throw new ArgumentNullException(nameof(clearance));
        _player = player != null ? player : throw new ArgumentNullException(nameof(player));
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public void Initialize()
    {
        _clearance.Cleared += OnCleared;
    }

    public void Dispose()
    {
        _clearance.Cleared -= OnCleared;
    }

    public void Tick()
    {
        if (!_isTraversing) return;

        _travelledDistance += _config.MovementSpeed * Time.deltaTime;

        _player.Transform.position = PositionAt(_travelledDistance);

        if (_travelledDistance < _spline.Length) return;

        _isTraversing = false;

        Finished?.Invoke();
    }

    private void OnCleared()
    {
        if (_isTraversing) return;
        if (!_spline.IsValid) return;

        _travelledDistance = 0f;
        _isTraversing = true;
    }

    private Vector3 PositionAt(float distance)
    {
        return _spline.EvaluateByDistance(distance) + Vector3.up * _playerSize.Radius;
    }
}
