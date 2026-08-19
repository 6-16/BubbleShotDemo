using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Gameplay/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] [Min(0.01f)] private float _initialRadius = 1f;

    [Tooltip("Below this radius the player is spent and the run is lost.")]
    [SerializeField] [Min(0.001f)] private float _minimumRadius = 0.15f;

    public float InitialRadius => _initialRadius;
    public float MinimumRadius => _minimumRadius;

    public float InitialVolume => BallVolume.FromRadius(_initialRadius);
    public float MinimumVolume => BallVolume.FromRadius(_minimumRadius);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_minimumRadius >= _initialRadius)
        {
            _minimumRadius = _initialRadius * 0.5f;
        }
    }
#endif
}
