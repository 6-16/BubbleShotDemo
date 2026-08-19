using UnityEngine;

public static class BallVolume
{
    private const float OneThird = 1f / 3f;

    private static readonly float SphereVolumeFactor = 4f * Mathf.PI / 3f;

    public static float FromRadius(float radius)
    {
        return SphereVolumeFactor * radius * radius * radius;
    }

    public static float ToRadius(float volume)
    {
        if (volume <= 0f) return 0f;

        return Mathf.Pow(volume / SphereVolumeFactor, OneThird);
    }
}
