using System;
using UnityEngine;
using Zenject;

public class PathClearance : IInitializable, IDisposable
{
    private const int MaximumOverlapResults = 32;

    private readonly PathSpline _spline;
    private readonly PlayerSize _playerSize;
    private readonly PlayerConfig _config;
    private readonly BlastResolver _blastResolver;
    private readonly LayerMask _obstacleMask;
    private readonly Collider[] _overlapBuffer = new Collider[MaximumOverlapResults];

    private bool _isClear;

    public event Action Cleared;

    public bool IsClear => _isClear;

    public PathClearance(
        PathSpline spline,
        PlayerSize playerSize,
        PlayerConfig config,
        BlastResolver blastResolver,
        LayerMask obstacleMask)
    {
        _spline = spline != null ? spline : throw new ArgumentNullException(nameof(spline));
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _blastResolver = blastResolver ?? throw new ArgumentNullException(nameof(blastResolver));
        _obstacleMask = obstacleMask;
    }

    public void Initialize()
    {
        _blastResolver.BlastResolved += OnBlastResolved;
        _playerSize.Changed += Evaluate;

        Evaluate();
    }

    public void Dispose()
    {
        _blastResolver.BlastResolved -= OnBlastResolved;
        _playerSize.Changed -= Evaluate;
    }

    public void Evaluate()
    {
        if (_isClear) return;
        if (!_spline.IsValid) return;

        if (IsBlocked()) return;

        _isClear = true;

        Cleared?.Invoke();
    }

    private bool IsBlocked()
    {
        float clearanceRadius = _playerSize.Radius * _config.ClearanceMultiplier;

        foreach (Vector3 sample in _spline.Samples)
        {
            int count = Physics.OverlapSphereNonAlloc(
                sample,
                clearanceRadius,
                _overlapBuffer,
                _obstacleMask,
                QueryTriggerInteraction.Ignore);

            if (count > 0) return true;
        }

        return false;
    }

    private void OnBlastResolved(Vector3 center, float radius)
    {
        Evaluate();
    }
}
