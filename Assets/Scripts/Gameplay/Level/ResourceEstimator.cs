using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEstimator
{
    private readonly List<Vector3> _uncovered = new List<Vector3>();

    public int EstimateShotCount(IReadOnlyList<Vector3> obstaclePositions, float blastRadius)
    {
        if (obstaclePositions == null) throw new ArgumentNullException(nameof(obstaclePositions));
        if (blastRadius <= 0f) throw new ArgumentOutOfRangeException(nameof(blastRadius));

        _uncovered.Clear();
        _uncovered.AddRange(obstaclePositions);

        float blastRadiusSqr = blastRadius * blastRadius;
        int shots = 0;

        while (_uncovered.Count > 0)
        {
            Vector3 center = FindBestCenter(blastRadiusSqr);

            RemoveCovered(center, blastRadiusSqr);

            shots++;
        }

        return shots;
    }

    public float EstimateRequiredVolume(IReadOnlyList<Vector3> obstaclePositions, LevelConfig config, float blastMultiplier)
    {
        float blastRadius = config.ReferenceProjectileRadius * blastMultiplier;
        int shots = EstimateShotCount(obstaclePositions, blastRadius);

        return shots * BallVolume.FromRadius(config.ReferenceProjectileRadius);
    }

    private Vector3 FindBestCenter(float blastRadiusSqr)
    {
        Vector3 bestCenter = _uncovered[0];
        int bestCount = 0;

        for (int index = 0; index < _uncovered.Count; index++)
        {
            int count = CountCovered(_uncovered[index], blastRadiusSqr);

            if (count <= bestCount) continue;

            bestCount = count;
            bestCenter = _uncovered[index];
        }

        return bestCenter;
    }

    private int CountCovered(Vector3 center, float blastRadiusSqr)
    {
        int count = 0;

        for (int index = 0; index < _uncovered.Count; index++)
        {
            if ((_uncovered[index] - center).sqrMagnitude <= blastRadiusSqr) count++;
        }

        return count;
    }

    private void RemoveCovered(Vector3 center, float blastRadiusSqr)
    {
        for (int index = _uncovered.Count - 1; index >= 0; index--)
        {
            if ((_uncovered[index] - center).sqrMagnitude > blastRadiusSqr) continue;

            _uncovered.RemoveAt(index);
        }
    }
}
