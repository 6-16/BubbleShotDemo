using Zenject;

public class ProjectilePool : MonoMemoryPool<Projectile>
{
    protected override void OnSpawned(Projectile projectile)
    {
        base.OnSpawned(projectile);

        projectile.ResetState();
    }
}
