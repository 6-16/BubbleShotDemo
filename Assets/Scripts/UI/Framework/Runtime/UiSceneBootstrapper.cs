using System;
using Zenject;

// Nothing is pre-placed in scenes, so the configured entry screen is opened on scene start.
public class UiSceneBootstrapper : IInitializable
{
    private readonly IUiService _uiService;

    public UiSceneBootstrapper(IUiService uiService)
    {
        _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));
    }

    public async void Initialize()
    {
        await _uiService.ShowEntryScreenAsync();
    }
}
