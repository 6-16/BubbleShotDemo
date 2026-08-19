using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Screen", menuName = "UI/Screen Definition")]
public class UiScreenDefinition : ScriptableObject
{
    [SerializeField] private UiScreen _prefab;
    [SerializeField] private ScreenKind _kind;

    [Tooltip("Higher values draw above lower ones. Screens 0, HUD 100, overlays 200.")]
    [SerializeField] private int _sortPriority;

    [SerializeField] private bool _preload;
    [SerializeField] private bool _destroyOnClose;

    [Tooltip("Refuses the cancel input, for prompts that must be answered.")]
    [SerializeField] private bool _blocksBack;

    public UiScreen Prefab => _prefab;
    public ScreenKind Kind => _kind;
    public int SortPriority => _sortPriority;
    public bool Preload => _preload;
    public bool DestroyOnClose => _destroyOnClose;
    public bool BlocksBack => _blocksBack;

    // The prefab's component type is the screen's identity, so lookups stay compile-checked.
    public Type ScreenType => _prefab != null ? _prefab.GetType() : null;
}
