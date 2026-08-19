using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private SceneCatalog _sceneCatalog;

    public override void InstallBindings()
    {
        Container.BindInstance(_sceneCatalog).AsSingle();

        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

        Container.BindInterfacesAndSelfTo<MainMenuState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();

        Container.Bind<AppStateMachine>().AsSingle();

        Container.BindInterfacesTo<AppBootstrapper>().AsSingle();
    }
}
