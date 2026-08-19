using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : IDisposable
{
    private readonly ProjectilePool _pool;
    private readonly ShotConfig _config;
    private readonly List<Projectile> _active = new List<Projectile>();

    public event Action<Projectile, Collider, Vector3> ProjectileHit;

    public ProjectileLauncher(ProjectilePool pool, ShotConfig config)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        _config = config != null ? config : throw new ArgumentNullException(nameof(config));
    }

    public Projectile Spawn(Vector3 position, float radius)
    {
        Projectile projectile = _pool.Spawn();

        projectile.Place(position);
        projectile.SetRadius(radius);

        _active.Add(projectile);

        return projectile;
    }

    public void Launch(Projectile projectile, Vector3 direction)
    {
        projectile.Hit += OnHit;
        projectile.Finished += OnFinished;

        projectile.Launch(direction, _config.ProjectileSpeed, _config.ProjectileLifetime);
    }

    public void Despawn(Projectile projectile)
    {
        if (!_active.Remove(projectile)) return;

        projectile.Hit -= OnHit;
        projectile.Finished -= OnFinished;

        _pool.Despawn(projectile);
    }

    public void Dispose()
    {
        for (int index = _active.Count - 1; index >= 0; index--)
        {
            Projectile projectile = _active[index];

            if (projectile == null) continue;

            projectile.Hit -= OnHit;
            projectile.Finished -= OnFinished;

            _pool.Despawn(projectile);
        }

        _active.Clear();
    }

    private void OnHit(Projectile projectile, Collider collider, Vector3 point)
    {
        ProjectileHit?.Invoke(projectile, collider, point);
    }

    private void OnFinished(Projectile projectile)
    {
        Despawn(projectile);
    }
}
