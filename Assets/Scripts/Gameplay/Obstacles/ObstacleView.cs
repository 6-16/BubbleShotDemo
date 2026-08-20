using System;
using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    private static readonly int BaseColorPropertyId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private Color _explodingColor = Color.white;
    [SerializeField] [Min(0.01f)] private float _explosionDuration = 0.2f;

    private MaterialPropertyBlock _propertyBlock;
    private Vector3 _initialScale;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _initialScale = _bodyTransform.localScale;

        _obstacle.Exploded += OnExploded;
    }

    private void OnDestroy()
    {
        _obstacle.Exploded -= OnExploded;
    }

    private async void OnExploded(Obstacle obstacle)
    {
        try
        {
            await PlayExplosionAsync();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async Awaitable PlayExplosionAsync()
    {
        ApplyColor(_explodingColor);

        if (_explosionParticles != null)
        {
            _explosionParticles.Play();
        }

        float elapsed = 0f;

        while (elapsed < _explosionDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / _explosionDuration);
            _bodyTransform.localScale = Vector3.Lerp(_initialScale, Vector3.zero, progress);

            await Awaitable.NextFrameAsync(destroyCancellationToken);
        }

        await WaitForParticlesAsync();

        if (this == null) return;

        gameObject.SetActive(false);
    }

    private async Awaitable WaitForParticlesAsync()
    {
        while (_explosionParticles != null && _explosionParticles.IsAlive(true))
        {
            await Awaitable.NextFrameAsync(destroyCancellationToken);
        }
    }

    private void ApplyColor(Color color)
    {
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(BaseColorPropertyId, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
