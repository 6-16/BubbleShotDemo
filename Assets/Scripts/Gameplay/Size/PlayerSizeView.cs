using System;
using UnityEngine;
using Zenject;

public class PlayerSizeView : MonoBehaviour
{
    [SerializeField] private Transform _ballTransform;

    [Tooltip("Radius of the ball mesh at unit scale. A default sphere is 0.5.")]
    [SerializeField] [Min(0.001f)] private float _meshRadiusAtUnitScale = 0.5f;

    private PlayerSize _playerSize;

    [Inject]
    private void Construct(PlayerSize playerSize)
    {
        _playerSize = playerSize ?? throw new ArgumentNullException(nameof(playerSize));

        _playerSize.Changed += OnSizeChanged;

        OnSizeChanged();
    }

    private void OnDestroy()
    {
        if (_playerSize == null) return;

        _playerSize.Changed -= OnSizeChanged;
    }

    private void OnSizeChanged()
    {
        float scale = _playerSize.Radius / _meshRadiusAtUnitScale;

        _ballTransform.localScale = new Vector3(scale, scale, scale);
    }
}
