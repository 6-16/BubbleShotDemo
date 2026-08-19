using System;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour
{
    private static readonly int OpenTriggerId = Animator.StringToHash("Open");

    [SerializeField] private Animator _animator;

    [Tooltip("Disabled box collider used purely as the editable detection volume.")]
    [SerializeField] private BoxCollider _detectionBounds;

    [SerializeField] private LayerMask _playerMask;

    private PathTraversal _traversal;
    private bool _isOpen;

    public event Action Opened;

    public bool IsOpen => _isOpen;

    [Inject]
    private void Construct(PathTraversal traversal)
    {
        _traversal = traversal ?? throw new ArgumentNullException(nameof(traversal));
    }

    private void Update()
    {
        if (_isOpen) return;
        if (!_traversal.IsTraversing) return;
        if (!IsPlayerInside()) return;

        Open();
    }

    private bool IsPlayerInside()
    {
        Transform boundsTransform = _detectionBounds.transform;

        Vector3 center = boundsTransform.TransformPoint(_detectionBounds.center);
        Vector3 halfExtents = Vector3.Scale(_detectionBounds.size * 0.5f, boundsTransform.lossyScale);

        return Physics.CheckBox(
            center,
            halfExtents,
            boundsTransform.rotation,
            _playerMask,
            QueryTriggerInteraction.Ignore);
    }

    private void Open()
    {
        _isOpen = true;

        _animator.SetTrigger(OpenTriggerId);

        Opened?.Invoke();
    }
}
