using System;

public class PlayerSize
{
    private readonly float _minimumVolume;

    private float _volume;
    private float _radius;

    public event Action Changed;

    public float Volume => _volume;
    public float Radius => _radius;
    public float MinimumVolume => _minimumVolume;
    public bool IsDepleted => _volume <= _minimumVolume;

    public PlayerSize(PlayerConfig config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        _minimumVolume = config.MinimumVolume;

        SetVolume(config.InitialVolume);
    }

    public float AvailableVolume => Math.Max(0f, _volume - _minimumVolume);

    public float Consume(float volume)
    {
        if (volume <= 0f) return 0f;

        float consumed = Math.Min(volume, _volume);

        SetVolume(_volume - consumed);

        return consumed;
    }

    private void SetVolume(float volume)
    {
        _volume = Math.Max(0f, volume);
        _radius = BallVolume.ToRadius(_volume);

        Changed?.Invoke();
    }
}
