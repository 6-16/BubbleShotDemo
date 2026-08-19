using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PathSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;

    [Tooltip("Points sampled along the spline for clearance checks and the corridor visual.")]
    [SerializeField] [Min(2)] private int _sampleCount = 64;

    private readonly List<Vector3> _samples = new List<Vector3>();

    private float _length;
    private bool _isBuilt;

    public IReadOnlyList<Vector3> Samples
    {
        get
        {
            EnsureBuilt();

            return _samples;
        }
    }

    public float Length
    {
        get
        {
            EnsureBuilt();

            return _length;
        }
    }

    public bool IsValid
    {
        get
        {
            EnsureBuilt();

            return _samples.Count > 1;
        }
    }

    public Vector3 End => Samples[_samples.Count - 1];

    public Vector3 EvaluateByDistance(float distance)
    {
        EnsureBuilt();

        if (!IsValid) return transform.position;

        float normalized = Mathf.Clamp01(_length <= 0f ? 0f : distance / _length);

        return _splineContainer.EvaluatePosition(normalized);
    }

    public void Rebuild()
    {
        _samples.Clear();
        _isBuilt = true;

        if (_splineContainer == null || _splineContainer.Spline == null) return;

        for (int index = 0; index < _sampleCount; index++)
        {
            float normalized = index / (float)(_sampleCount - 1);

            _samples.Add(_splineContainer.EvaluatePosition(normalized));
        }

        _length = _splineContainer.CalculateLength();
    }

    private void EnsureBuilt()
    {
        if (_isBuilt) return;

        Rebuild();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Rebuild();

        Gizmos.color = Color.cyan;

        for (int index = 0; index < _samples.Count - 1; index++)
        {
            Gizmos.DrawLine(_samples[index], _samples[index + 1]);
        }
    }
#endif
}
