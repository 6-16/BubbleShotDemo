using UnityEngine;

public interface IUiService
{
    Awaitable<TScreen> ShowAsync<TScreen>() where TScreen : UiScreen;
    Awaitable<TScreen> ShowAsync<TScreen, TArgs>(TArgs args) where TScreen : UiScreenWithArgs<TArgs>;
    Awaitable CloseAsync<TScreen>() where TScreen : UiScreen;
    Awaitable CloseTopAsync();
    Awaitable BackAsync();
    Awaitable CloseAllAsync();
    Awaitable ShowEntryScreenAsync();

    bool IsOpen<TScreen>() where TScreen : UiScreen;
    TScreen Get<TScreen>() where TScreen : UiScreen;
}
