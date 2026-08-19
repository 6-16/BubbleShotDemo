using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Gameplay/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] [Min(0.01f)] private float _initialRadius = 1f;

    [Tooltip("Below this radius the player is spent and the run is lost.")]
    [SerializeField] [Min(0.001f)] private float _minimumRadius = 0.15f;

    [SerializeField] [Min(0.1f)] private float _movementSpeed = 6f;

    [Tooltip("Corridor clearance radius as a multiple of the player radius.")]
    [SerializeField] [Min(1f)] private float _clearanceMultiplier = 1.1f;

    public float InitialRadius => _initialRadius;
    public float MovementSpeed => _movementSpeed;
    public float ClearanceMultiplier => _clearanceMultiplier;
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
