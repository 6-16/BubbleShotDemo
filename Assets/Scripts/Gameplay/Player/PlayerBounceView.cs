using System;
using UnityEngine;
using Zenject;

public class PlayerBounceView : MonoBehaviour
{
    private static readonly int IsMovingParameterId = Animator.StringToHash("IsMoving");

    [SerializeField] private Animator _animator;

    private PathTraversal _traversal;

    [Inject]
    private void Construct(PathTraversal traversal)
    {
        _traversal = traversal ?? throw new ArgumentNullException(nameof(traversal));

        _traversal.Started += OnStarted;
        _traversal.Finished += OnFinished;
    }

    private void OnDestroy()
    {
        if (_traversal == null) return;

        _traversal.Started -= OnStarted;
        _traversal.Finished -= OnFinished;
    }

    private void OnStarted()
    {
        _animator.SetBool(IsMovingParameterId, true);
    }

    private void OnFinished()
    {
        _animator.SetBool(IsMovingParameterId, false);
    }
}
