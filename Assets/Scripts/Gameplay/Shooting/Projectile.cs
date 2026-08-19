using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform _ballTransform;

    [Tooltip("Radius of the ball mesh at unit scale. A default sphere is 0.5.")]
    [SerializeField] [Min(0.001f)] private float _meshRadiusAtUnitScale = 0.5f;

    [SerializeField] private LayerMask _hitMask;

    private Vector3 _direction;
    private float _radius;
    private float _speed;
    private float _lifetime;
    private float _elapsed;
    private bool _isFlying;

    public event Action<Projectile, Collider, Vector3> Hit;
    public event Action<Projectile> Finished;

    public float Radius => _radius;
    public float Volume => BallVolume.FromRadius(_radius);

    public void Place(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRadius(float radius)
    {
        _radius = Mathf.Max(0f, radius);

        float scale = _radius / _meshRadiusAtUnitScale;

        _ballTransform.localScale = new Vector3(scale, scale, scale);
    }

    public void Launch(Vector3 direction, float speed, float lifetime)
    {
        _direction = direction.normalized;
        _speed = speed;
        _lifetime = lifetime;
        _elapsed = 0f;
        _isFlying = true;
    }

    public void ResetState()
    {
        _isFlying = false;
        _elapsed = 0f;
        _direction = Vector3.forward;
    }

    private void Update()
    {
        if (!_isFlying) return;

        float step = _speed * Time.deltaTime;

        if (TryHit(step)) return;

        transform.position += _direction * step;
        _elapsed += Time.deltaTime;

        if (_elapsed < _lifetime) return;

        _isFlying = false;
        Finished?.Invoke(this);
    }

    private bool TryHit(float step)
    {
        bool isBlocked = Physics.SphereCast(
            transform.position,
            _radius,
            _direction,
            out RaycastHit hit,
            step,
            _hitMask,
            QueryTriggerInteraction.Ignore);

        if (!isBlocked) return false;

        _isFlying = false;

        Hit?.Invoke(this, hit.collider, hit.point);
        Finished?.Invoke(this);

        return true;
    }
}
