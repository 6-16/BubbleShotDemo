using UnityEngine;

[CreateAssetMenu(fileName = "ShotConfig", menuName = "Gameplay/Shot Config")]
public class ShotConfig : ScriptableObject
{
    [Tooltip("Projectile radius gained per second of holding.")]
    [SerializeField] [Min(0.01f)] private float _growthRatePerSecond = 0.35f;

    [SerializeField] [Min(0.01f)] private float _startRadius = 0.1f;

    [SerializeField] [Min(0.1f)] private float _projectileSpeed = 18f;

    [Tooltip("Blast radius as a multiple of the projectile radius.")]
    [SerializeField] [Min(1f)] private float _blastRadiusMultiplier = 2.5f;

    [Tooltip("Seconds a launched projectile survives before it is returned to the pool.")]
    [SerializeField] [Min(0.1f)] private float _projectileLifetime = 4f;

    [Tooltip("Half angle of the forward aiming cone, in degrees.")]
    [SerializeField] [Range(1f, 89f)] private float _aimConeHalfAngle = 45f;

    public float GrowthRatePerSecond => _growthRatePerSecond;
    public float StartRadius => _startRadius;
    public float ProjectileSpeed => _projectileSpeed;
    public float BlastRadiusMultiplier => _blastRadiusMultiplier;
    public float ProjectileLifetime => _projectileLifetime;
    public float AimConeHalfAngle => _aimConeHalfAngle;
}
