using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private ShotConfig _shotConfig;
    [SerializeField] private PlayerSizeView _playerSizeView;

    public override void InstallBindings()
    {
        Container.BindInstance(_playerConfig).AsSingle();
        Container.BindInstance(_shotConfig).AsSingle();

        Container.Bind<PlayerSize>().AsSingle().NonLazy();

        Container.BindInstance(_playerSizeView).AsSingle();
        Container.QueueForInject(_playerSizeView);
    }
}
