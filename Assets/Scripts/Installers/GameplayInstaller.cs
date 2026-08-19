using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private ShotConfig _shotConfig;
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private PlayerSizeView _playerSizeView;
    [SerializeField] private ShotInputReader _shotInputReader;
    [SerializeField] private AimIndicatorView _aimIndicatorView;
    [SerializeField] private Camera _gameplayCamera;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _projectileParent;
    [SerializeField] [Min(1)] private int _projectilePoolSize = 8;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private PathSpline _pathSpline;
    [SerializeField] private PathView _pathView;

    public override void InstallBindings()
    {
        Container.BindInstance(_playerConfig).AsSingle();
        Container.BindInstance(_shotConfig).AsSingle();

        Container.Bind<PlayerSize>().AsSingle().NonLazy();

        Container.BindInstance(_playerRoot).AsSingle();
        Container.BindInstance(_gameplayCamera).AsSingle();

        Container.Bind<IShotInput>().FromInstance(_shotInputReader).AsSingle();
        Container.BindInterfacesAndSelfTo<ShotAimer>().AsSingle();

        Container.BindMemoryPool<Projectile, ProjectilePool>()
            .WithInitialSize(_projectilePoolSize)
            .FromComponentInNewPrefab(_projectilePrefab)
            .UnderTransform(_projectileParent);

        Container.BindInterfacesAndSelfTo<ProjectileLauncher>().AsSingle();
        Container.BindInterfacesAndSelfTo<ShotCharger>().AsSingle();

        Container.BindInterfacesAndSelfTo<BlastResolver>().AsSingle().WithArguments(_obstacleMask);

        Container.BindInstance(_pathSpline).AsSingle();
        Container.BindInterfacesAndSelfTo<PathClearance>().AsSingle().WithArguments(_obstacleMask);
        Container.BindInterfacesAndSelfTo<PathTraversal>().AsSingle();

        Container.BindInstance(_pathView).AsSingle();
        Container.QueueForInject(_pathView);

        Container.BindInstance(_playerSizeView).AsSingle();
        Container.BindInstance(_aimIndicatorView).AsSingle();

        Container.QueueForInject(_playerSizeView);
        Container.QueueForInject(_aimIndicatorView);
    }
}
