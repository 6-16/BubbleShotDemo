using UnityEngine;

[CreateAssetMenu(fileName = "SceneCatalog", menuName = "App/Scene Catalog")]
public class SceneCatalog : ScriptableObject
{
    [SerializeField] private SceneReference _loading;
    [SerializeField] private SceneReference _menu;
    [SerializeField] private SceneReference _gameplay;

    public SceneReference Loading => _loading;
    public SceneReference Menu => _menu;
    public SceneReference Gameplay => _gameplay;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _loading.SyncFromAsset();
        _menu.SyncFromAsset();
        _gameplay.SyncFromAsset();
    }
#endif
}
