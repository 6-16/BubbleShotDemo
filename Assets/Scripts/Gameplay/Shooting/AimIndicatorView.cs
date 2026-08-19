using System;
using UnityEngine;
using Zenject;

public class AimIndicatorView : MonoBehaviour
{
    [SerializeField] private Transform _indicator;

    private ShotAimer _aimer;

    [Inject]
    private void Construct(ShotAimer aimer)
    {
        _aimer = aimer ?? throw new ArgumentNullException(nameof(aimer));

        _aimer.DirectionChanged += OnDirectionChanged;
    }

    private void OnDestroy()
    {
        if (_aimer == null) return;

        _aimer.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged()
    {
        _indicator.rotation = Quaternion.LookRotation(_aimer.Direction, Vector3.up);
    }
}
