using System.Collections.Generic;
using System.Linq;

public class UiNavigationStack
{
    private readonly List<UiScreen> _entries = new List<UiScreen>();

    public int Count => _entries.Count;
    public bool IsEmpty => _entries.Count == 0;
    public UiScreen Top => _entries.Count > 0 ? _entries[_entries.Count - 1] : null;

    public IReadOnlyList<UiScreen> Entries => _entries;

    public void Push(UiScreen screen)
    {
        _entries.Add(screen);
    }

    public void Remove(UiScreen screen)
    {
        _entries.Remove(screen);
    }

    public UiScreen Find<TScreen>() where TScreen : UiScreen
    {
        return _entries.FirstOrDefault(entry => entry is TScreen);
    }

    public IReadOnlyList<UiScreen> TakeAll()
    {
        List<UiScreen> copy = new List<UiScreen>(_entries);
        _entries.Clear();

        return copy;
    }
}
