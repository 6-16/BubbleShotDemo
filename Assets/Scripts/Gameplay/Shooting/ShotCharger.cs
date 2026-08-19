using System;
using UnityEngine;
using Zenject;

public class ShotCharger : IInitializable, ITickable, IDisposable
{
    private readonly IShotInput _input;
    private readonly ShotAimer _aimer;
    private readonly PlayerSize _playerSize;
    private readonly PlayerRoot _player;
    private readonly ProjectileLauncher _launcher;
    private readonly PathClearance _clearance;
    private readonly ShotConfig _config;

    private Projectile _charging;
    private float _radius;

    public bool IsCharging => _charging != null;

    public ShotCharger(
        IShotInput input,
        ShotAimer aimer,
        PlayerSize playerSize,
        PlayerRoot player,
        ProjectileLauncher launcher,
        PathClearance clearance,
        ShotConfig config)
    {
        _input = input ?? throw new ArgumentNullException(nameof(input));
        _aimer = aimer ?? throw new ArgumentNullException(nameof(aimer));
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));
        _player = player != null ? player : throw new ArgumentNullException(nameof(player));
        _launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
        _clearance = clearance ?? throw new ArgumentNullException(nameof(clearance));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public void Initialize()
    {
        _input.Began += OnBegan;
        _input.Released += OnReleased;
        _clearance.Cleared += OnPathCleared;
    }

    public void Dispose()
    {
        _input.Began -= OnBegan;
        _input.Released -= OnReleased;
        _clearance.Cleared -= OnPathCleared;
    }

    public void Tick()
    {
        if (_charging == null) return;

        _radius = Mathf.Min(_radius + _config.GrowthRatePerSecond * Time.deltaTime, MaximumRadius());

        _charging.SetRadius(_radius);
        _charging.Place(ChargePosition());
    }

    private void OnBegan()
    {
        if (_charging != null) return;
        if (_clearance.IsClear) return;
        if (_playerSize.IsDepleted) return;

        _radius = Mathf.Min(_config.StartRadius, MaximumRadius());
        _charging = _launcher.Spawn(ChargePosition(), _radius);
    }

    private void OnPathCleared()
    {
        if (_charging == null) return;

        _launcher.Despawn(_charging);

        _charging = null;
    }

    private void OnReleased()
    {
        if (_charging == null) return;

        Projectile projectile = _charging;
        _charging = null;

        _playerSize.Consume(projectile.Volume);

        _launcher.Launch(projectile, _aimer.Direction);
    }

    private float MaximumRadius()
    {
        return BallVolume.ToRadius(_playerSize.Volume);
    }

    private Vector3 ChargePosition()
    {
        return _player.Position + _aimer.Direction * (_playerSize.Radius + _radius);
    }
}
