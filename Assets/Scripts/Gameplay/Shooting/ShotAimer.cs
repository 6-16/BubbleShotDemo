using System;
using UnityEngine;
using Zenject;

public class ShotAimer : IInitializable, IDisposable
{
    private const float MinimumAimDistanceSqr = 0.0001f;

    private readonly IShotInput _input;
    private readonly PlayerRoot _player;
    private readonly Camera _camera;
    private readonly ShotConfig _config;

    private Vector3 _direction;

    public event Action DirectionChanged;

    public Vector3 Direction => _direction;

    public ShotAimer(IShotInput input, PlayerRoot player, Camera camera, ShotConfig config)
    {
        _input = input ?? throw new ArgumentNullException(nameof(input));
        _player = player != null ? player : throw new ArgumentNullException(nameof(player));
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public void Initialize()
    {
        _direction = FlattenedForward();

        _input.Aimed += OnAimed;
    }

    public void Dispose()
    {
        _input.Aimed -= OnAimed;
    }

    private void OnAimed(Vector2 screenPosition)
    {
        if (!TryGetGroundPoint(screenPosition, out Vector3 groundPoint)) return;

        Vector3 toPoint = groundPoint - _player.Position;
        toPoint.y = 0f;

        if (toPoint.sqrMagnitude < MinimumAimDistanceSqr) return;

        _direction = ClampToCone(toPoint.normalized);

        DirectionChanged?.Invoke();
    }

    private bool TryGetGroundPoint(Vector2 screenPosition, out Vector3 groundPoint)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane ground = new Plane(Vector3.up, _player.Position);

        if (ground.Raycast(ray, out float distance))
        {
            groundPoint = ray.GetPoint(distance);

            return true;
        }

        groundPoint = default;

        return false;
    }

    private Vector3 ClampToCone(Vector3 direction)
    {
        Vector3 forward = FlattenedForward();
        float angle = Vector3.SignedAngle(forward, direction, Vector3.up);
        float clampedAngle = Mathf.Clamp(angle, -_config.AimConeHalfAngle, _config.AimConeHalfAngle);

        return Quaternion.AngleAxis(clampedAngle, Vector3.up) * forward;
    }

    private Vector3 FlattenedForward()
    {
        Vector3 forward = _player.Forward;
        forward.y = 0f;

        return forward.sqrMagnitude < MinimumAimDistanceSqr ? Vector3.forward : forward.normalized;
    }
}
