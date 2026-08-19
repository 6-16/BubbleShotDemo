using UnityEngine;

public class FadeScreenTransition : ScreenTransition
{
    [SerializeField] private float _duration = 0.2f;

    public override Awaitable PlayShowAsync(CanvasGroup canvasGroup)
    {
        return FadeAsync(canvasGroup, canvasGroup.alpha, 1f);
    }

    public override Awaitable PlayHideAsync(CanvasGroup canvasGroup)
    {
        return FadeAsync(canvasGroup, canvasGroup.alpha, 0f);
    }

    // Unscaled time so menus still animate while the game is paused.
    private async Awaitable FadeAsync(CanvasGroup canvasGroup, float from, float to)
    {
        if (_duration <= 0f)
        {
            canvasGroup.alpha = to;
            return;
        }

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _duration);

            await Awaitable.NextFrameAsync();
        }

        canvasGroup.alpha = to;
    }
}
