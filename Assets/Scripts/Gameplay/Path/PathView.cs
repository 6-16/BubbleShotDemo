using System;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

public class PathView : MonoBehaviour
{
    private const float RoadWidthAtUnitRadius = 1.2f;

    [SerializeField] private SplineExtrude _splineExtrude;

    private PlayerSize _playerSize;

    [Inject]
    private void Construct(PlayerSize playerSize)
    {
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));

        _playerSize.Changed += OnPlayerSizeChanged;

        OnPlayerSizeChanged();
    }

    private void OnDestroy()
    {
        if (_playerSize == null) return;

        _playerSize.Changed -= OnPlayerSizeChanged;
    }

    private void OnPlayerSizeChanged()
    {
        float targetWidth = _playerSize.Radius * 2f;

        _splineExtrude.Radius = targetWidth / RoadWidthAtUnitRadius;
        _splineExtrude.Rebuild();
    }
}
