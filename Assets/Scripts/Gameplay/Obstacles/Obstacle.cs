using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    [Tooltip("Gameplay radius used by clearance and budget calculations.")]
    [SerializeField] [Min(0.01f)] private float _radius = 0.5f;

    private bool _isAlive = true;

    public event Action<Obstacle> Exploded;

    public bool IsAlive => _isAlive;
    public float Radius => _radius;
    public Vector3 Position => transform.position;

    public void Explode()
    {
        if (!_isAlive) return;

        _isAlive = false;
        _collider.enabled = false;

        Exploded?.Invoke(this);
    }
}
