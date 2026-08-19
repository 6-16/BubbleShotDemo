using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Gameplay/Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private Obstacle _obstaclePrefab;
    [SerializeField] private int _seed = 1;

    [Tooltip("Distance between grid cells before jitter is applied.")]
    [SerializeField] [Min(0.1f)] private float _spacing = 1.5f;

    [Tooltip("Maximum random offset applied to each grid point, as a fraction of spacing.")]
    [SerializeField] [Range(0f, 1f)] private float _jitter = 0.35f;

    [Tooltip("Chance that a grid cell produces an obstacle at all.")]
    [SerializeField] [Range(0f, 1f)] private float _fillChance = 0.9f;

    [Tooltip("Projectile radius the budget estimate assumes for a typical shot.")]
    [SerializeField] [Min(0.01f)] private float _referenceProjectileRadius = 0.5f;

    [Tooltip("Extra player resource above the estimated requirement.")]
    [SerializeField] [Range(0f, 1f)] private float _reserveFraction = 0.2f;

    public Obstacle ObstaclePrefab => _obstaclePrefab;
    public int Seed => _seed;
    public float Spacing => _spacing;
    public float Jitter => _jitter;
    public float FillChance => _fillChance;
    public float ReferenceProjectileRadius => _referenceProjectileRadius;
    public float ReserveFraction => _reserveFraction;
}
