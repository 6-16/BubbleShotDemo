using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ObstacleGenerator
{
    public void Generate(LevelConfig config, Bounds bounds, List<Vector3> results)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (results == null) throw new ArgumentNullException(nameof(results));

        results.Clear();

        Random random = new Random(config.Seed);

        int columns = Mathf.FloorToInt(bounds.size.x / config.Spacing);
        int rows = Mathf.FloorToInt(bounds.size.z / config.Spacing);

        for (int column = 0; column <= columns; column++)
        {
            for (int row = 0; row <= rows; row++)
            {
                if (random.NextDouble() > config.FillChance) continue;

                results.Add(CreatePosition(config, bounds, random, column, row));
            }
        }
    }

    private Vector3 CreatePosition(
        LevelConfig config,
        Bounds bounds,
        Random random,
        int column,
        int row)
    {
        float jitterRange = config.Spacing * config.Jitter;

        float x = bounds.min.x + column * config.Spacing + NextRange(random, -jitterRange, jitterRange);
        float z = bounds.min.z + row * config.Spacing + NextRange(random, -jitterRange, jitterRange);

        return new Vector3(
            Mathf.Clamp(x, bounds.min.x, bounds.max.x),
            bounds.center.y,
            Mathf.Clamp(z, bounds.min.z, bounds.max.z));
    }

    private float NextRange(Random random, float minimum, float maximum)
    {
        return minimum + (float)random.NextDouble() * (maximum - minimum);
    }
}
