using System;
using UnityEngine;
using Zenject;

public class UiScreenFactory
{
    private readonly DiContainer _container;
    private readonly UiRootView _rootView;

    public UiScreenFactory(DiContainer container, UiRootView rootView)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _rootView = rootView != null ? rootView : throw new ArgumentNullException(nameof(rootView));
    }

    public UiScreen Create(UiScreenDefinition definition)
    {
        UiScreen screen = _container.InstantiatePrefabForComponent<UiScreen>(
            definition.Prefab,
            _rootView.ScreenContainer);

        screen.Bind(definition);
        screen.SetVisible(false);
        screen.SetInteractable(false);
        screen.GameObject.SetActive(false);

        return screen;
    }

    // Sibling order is the draw order inside a single canvas, so priority maps straight onto it.
    public void ApplySortPriority(UiScreen screen)
    {
        int priority = screen.Definition.SortPriority;
        int index = 0;

        foreach (Transform sibling in _rootView.ScreenContainer)
        {
            if (sibling == screen.transform) continue;

            UiScreen other = sibling.GetComponent<UiScreen>();

            if (other == null || other.Definition == null) continue;
            if (other.Definition.SortPriority > priority) break;

            index++;
        }

        screen.transform.SetSiblingIndex(index);
    }
}
