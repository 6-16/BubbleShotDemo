using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObstacleSpawnZone : MonoBehaviour
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private ShotConfig _shotConfig;
    [SerializeField] private Vector2 _size = new Vector2(20f, 30f);
    [SerializeField] private Transform _obstacleParent;

    [Header("Budget readout")]
    [SerializeField] private int _spawnedCount;
    [SerializeField] private int _estimatedShotCount;
    [SerializeField] private float _estimatedRequiredVolume;
    [SerializeField] private float _recommendedInitialRadius;

    private readonly List<Vector3> _positions = new List<Vector3>();

    public Bounds Bounds => new Bounds(transform.position, new Vector3(_size.x, 0f, _size.y));

#if UNITY_EDITOR
    [ContextMenu("Generate Obstacles")]
    private void GenerateObstacles()
    {
        ClearObstacles();

        new ObstacleGenerator().Generate(_levelConfig, Bounds, _positions);

        foreach (Vector3 position in _positions)
        {
            Spawn(position);
        }

        _spawnedCount = _positions.Count;

        EstimateBudget();

        EditorUtility.SetDirty(this);
    }

    [ContextMenu("Clear Obstacles")]
    private void ClearObstacles()
    {
        for (int index = _obstacleParent.childCount - 1; index >= 0; index--)
        {
            DestroyImmediate(_obstacleParent.GetChild(index).gameObject);
        }

        _spawnedCount = 0;
    }

    private void Spawn(Vector3 position)
    {
        Obstacle obstacle = (Obstacle)PrefabUtility.InstantiatePrefab(_levelConfig.ObstaclePrefab, _obstacleParent);

        obstacle.transform.position = position;
    }

    private void EstimateBudget()
    {
        ResourceEstimator estimator = new ResourceEstimator();

        _estimatedShotCount = estimator.EstimateShotCount(
            _positions,
            _levelConfig.ReferenceProjectileRadius * _shotConfig.BlastRadiusMultiplier);

        _estimatedRequiredVolume = estimator.EstimateRequiredVolume(
            _positions,
            _levelConfig,
            _shotConfig.BlastRadiusMultiplier);

        float initialVolume = _estimatedRequiredVolume * (1f + _levelConfig.ReserveFraction);

        _recommendedInitialRadius = BallVolume.ToRadius(initialVolume);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_size.x, 0.1f, _size.y));
    }
#endif
}
