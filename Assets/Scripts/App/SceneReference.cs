using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneReference
{
    [SerializeField] private string _sceneName;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
#endif

    public string SceneName => _sceneName;

    public bool IsAssigned => !string.IsNullOrEmpty(_sceneName);

#if UNITY_EDITOR
    public void SyncFromAsset()
    {
        _sceneName = _sceneAsset != null ? _sceneAsset.name : string.Empty;
    }
#endif
}
