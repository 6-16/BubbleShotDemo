using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotInputReader : MonoBehaviour, IShotInput
{
    [SerializeField] private InputActionReference _shotAction;
    [SerializeField] private InputActionReference _pointerPositionAction;

    private bool _isHeld;

    public event Action Began;
    public event Action<Vector2> Aimed;
    public event Action Released;

    public bool IsHeld => _isHeld;

    private void OnEnable()
    {
        _shotAction.action.started += OnShotStarted;
        _shotAction.action.canceled += OnShotCanceled;
        _pointerPositionAction.action.performed += OnPointerMoved;

        _shotAction.action.Enable();
        _pointerPositionAction.action.Enable();
    }

    private void OnDisable()
    {
        _shotAction.action.started -= OnShotStarted;
        _shotAction.action.canceled -= OnShotCanceled;
        _pointerPositionAction.action.performed -= OnPointerMoved;

        _shotAction.action.Disable();
        _pointerPositionAction.action.Disable();
    }

    private void OnShotStarted(InputAction.CallbackContext context)
    {
        _isHeld = true;

        Aimed?.Invoke(ReadPointerPosition());
        Began?.Invoke();
    }

    private void OnShotCanceled(InputAction.CallbackContext context)
    {
        if (!_isHeld) return;

        _isHeld = false;
        Released?.Invoke();
    }

    private void OnPointerMoved(InputAction.CallbackContext context)
    {
        Aimed?.Invoke(context.ReadValue<Vector2>());
    }

    private Vector2 ReadPointerPosition()
    {
        return _pointerPositionAction.action.ReadValue<Vector2>();
    }
}
