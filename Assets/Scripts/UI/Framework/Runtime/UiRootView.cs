using UnityEngine;

public class UiRootView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _screenContainer;
    [SerializeField] private UiInputBlocker _inputBlocker;

    public Canvas Canvas => _canvas;
    public RectTransform ScreenContainer => _screenContainer;
    public UiInputBlocker InputBlocker => _inputBlocker;
}
