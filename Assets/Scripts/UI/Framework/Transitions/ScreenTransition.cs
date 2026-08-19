using UnityEngine;

public abstract class ScreenTransition : MonoBehaviour
{
    public abstract Awaitable PlayShowAsync(CanvasGroup canvasGroup);
    public abstract Awaitable PlayHideAsync(CanvasGroup canvasGroup);
}
