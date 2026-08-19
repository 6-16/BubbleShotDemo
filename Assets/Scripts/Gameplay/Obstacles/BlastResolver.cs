using System;
using UnityEngine;
using Zenject;

public class BlastResolver : IInitializable, IDisposable
{
    private const int MaximumBlastTargets = 256;

    private readonly ProjectileLauncher _launcher;
    private readonly ShotConfig _config;
    private readonly LayerMask _obstacleMask;
    private readonly Collider[] _overlapBuffer = new Collider[MaximumBlastTargets];

    public event Action<Vector3, float> BlastResolved;

    public BlastResolver(ProjectileLauncher launcher, ShotConfig config, LayerMask obstacleMask)
    {
        _launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
        _obstacleMask = obstacleMask;
    }

    public void Initialize()
    {
        _launcher.ProjectileHit += OnProjectileHit;
    }

    public void Dispose()
    {
        _launcher.ProjectileHit -= OnProjectileHit;
    }

    private void OnProjectileHit(Projectile projectile, Collider collider, Vector3 point)
    {
        Obstacle hitObstacle = collider.GetComponentInParent<Obstacle>();

        if (hitObstacle == null) return;

        float blastRadius = projectile.Radius * _config.BlastRadiusMultiplier;

        Resolve(hitObstacle.Position, blastRadius);
    }

    private void Resolve(Vector3 center, float radius)
    {
        int count = Physics.OverlapSphereNonAlloc(
            center,
            radius,
            _overlapBuffer,
            _obstacleMask,
            QueryTriggerInteraction.Ignore);

        for (int index = 0; index < count; index++)
        {
            Obstacle obstacle = _overlapBuffer[index].GetComponentInParent<Obstacle>();

            if (obstacle == null) continue;
            if (!obstacle.IsAlive) continue;

            obstacle.Explode();
        }

        BlastResolved?.Invoke(center, radius);
    }
}
