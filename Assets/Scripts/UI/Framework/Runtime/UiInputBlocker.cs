using UnityEngine;

// Swallows input while a transition runs so a double tap cannot desync the navigation stack.
public class UiInputBlocker : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private int _blockCount;

    public void Block()
    {
        _blockCount++;
        Apply();
    }

    public void Unblock()
    {
        _blockCount = Mathf.Max(0, _blockCount - 1);
        Apply();
    }

    private void Apply()
    {
        _canvasGroup.blocksRaycasts = _blockCount > 0;
    }
}
