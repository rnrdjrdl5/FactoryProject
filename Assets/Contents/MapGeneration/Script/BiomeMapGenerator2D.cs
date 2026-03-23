using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

// 높이, 습도, 온도 노이즈를 이용해 2D 바이옴 맵을 생성한다.
public class BiomeMapGenerator2D : MonoBehaviour
{
    [Header("Map Size")]
    [SerializeField] int width = 128;
    [SerializeField] int height = 128;
    [SerializeField] float cellSize = 1f;

    [Header("Seed")]
    [SerializeField] bool useRandomSeed = true;
    [SerializeField] int seed = 12345;

    [Header("Base Split")]
    [SerializeField, Range(0f, 1f)] float oceanThreshold = 0.32f;
    [SerializeField] BiomeType defaultLandBiome = BiomeType.Grassland;

    [Header("Height Noise")]
    [SerializeField] float heightScale = 0.025f;
    [SerializeField] int heightOctaves = 4;
    [SerializeField, Range(0f, 1f)] float heightPersistence = 0.5f;
    [SerializeField] float heightLacunarity = 2f;

    [Header("Moisture Noise")]
    [SerializeField] float moistureScale = 0.03f;
    [SerializeField] int moistureOctaves = 3;
    [SerializeField, Range(0f, 1f)] float moisturePersistence = 0.5f;
    [SerializeField] float moistureLacunarity = 2f;

    [Header("Temperature Noise")]
    [SerializeField] float temperatureScale = 0.02f;
    [SerializeField] int temperatureOctaves = 3;
    [SerializeField, Range(0f, 1f)] float temperaturePersistence = 0.5f;
    [SerializeField] float temperatureLacunarity = 2f;

    [Header("Cleanup")]
    [SerializeField, Min(0)] int cleanupPasses = 2;
    [SerializeField, Range(0, 8)] int minimumSameBiomeNeighbors = 3;

    [Header("Clusters")]
    [SerializeField] bool clusterIncludesDiagonals;

    [Header("Biome Rules")]
    [SerializeField] BiomeRule[] biomeRules = new BiomeRule[0];

    BiomeMapData generatedMap;
    BiomeClusterMapData generatedClusters;

    public BiomeMapData GeneratedMap => generatedMap;
    public BiomeClusterMapData GeneratedClusters => generatedClusters;

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        generatedMap = new BiomeMapData(width, height, seed, defaultLandBiome);

        System.Random random = new System.Random(seed);
        Vector2 heightOffset = CreateOffset(random);
        Vector2 moistureOffset = CreateOffset(random);
        Vector2 temperatureOffset = CreateOffset(random);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightValue = BiomeNoiseUtility.SampleFractalNoise(
                    x, y, heightScale, heightOctaves, heightPersistence, heightLacunarity, heightOffset);

                float moistureValue = BiomeNoiseUtility.SampleFractalNoise(
                    x, y, moistureScale, moistureOctaves, moisturePersistence, moistureLacunarity, moistureOffset);

                float temperatureValue = BiomeNoiseUtility.SampleFractalNoise(
                    x, y, temperatureScale, temperatureOctaves, temperaturePersistence, temperatureLacunarity, temperatureOffset);

                BiomeType biome = ResolveBiome(heightValue, moistureValue, temperatureValue);
                generatedMap.SetBiome(x, y, biome);
            }
        }

        for (int i = 0; i < cleanupPasses; i++)
        {
            CleanupSmallBiomeChunks();
        }

        BuildClusters();
    }

    [ContextMenu("Build Clusters")]
    public void BuildClusters()
    {
        generatedClusters = BiomeClusterBuilder.Build(generatedMap, clusterIncludesDiagonals);
    }

    public BiomeType GetBiomeAtWorldPosition(Vector3 worldPosition)
    {
        if (generatedMap == null)
        {
            return defaultLandBiome;
        }

        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Vector2Int cell = generatedMap.ToCell(localPosition, cellSize);
        return generatedMap.GetBiome(cell.x, cell.y);
    }

    public int GetClusterIdAtCell(Vector2Int cell)
    {
        if (generatedClusters == null)
        {
            return -1;
        }

        return generatedClusters.GetClusterId(cell.x, cell.y);
    }

    public int GetClusterIdAtWorldPosition(Vector3 worldPosition)
    {
        if (generatedMap == null || generatedClusters == null)
        {
            return -1;
        }

        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Vector2Int cell = generatedMap.ToCell(localPosition, cellSize);
        return generatedClusters.GetClusterId(cell.x, cell.y);
    }

    public bool TryGetClusterAtCell(Vector2Int cell, out BiomeCluster cluster)
    {
        if (generatedClusters == null)
        {
            cluster = null;
            return false;
        }

        return generatedClusters.TryGetCluster(cell.x, cell.y, out cluster);
    }

    public bool TryGetCluster(int clusterId, out BiomeCluster cluster)
    {
        if (generatedClusters == null)
        {
            cluster = null;
            return false;
        }

        return generatedClusters.TryGetCluster(clusterId, out cluster);
    }

    public IReadOnlyList<Vector2Int> GetCellsInCluster(int clusterId)
    {
        if (generatedClusters == null)
        {
            return new Vector2Int[0];
        }

        return generatedClusters.GetCells(clusterId);
    }

    BiomeType ResolveBiome(float heightValue, float moistureValue, float temperatureValue)
    {
        if (heightValue < oceanThreshold)
        {
            return BiomeType.Ocean;
        }

        BiomeRule selectedRule = null;

        for (int i = 0; i < biomeRules.Length; i++)
        {
            BiomeRule rule = biomeRules[i];
            if (rule == null || rule.biomeType == BiomeType.Ocean)
            {
                continue;
            }

            if (!rule.Matches(heightValue, moistureValue, temperatureValue))
            {
                continue;
            }

            if (selectedRule == null || rule.priority > selectedRule.priority)
            {
                selectedRule = rule;
            }
        }

        return selectedRule != null ? selectedRule.biomeType : defaultLandBiome;
    }

    void CleanupSmallBiomeChunks()
    {
        if (generatedMap == null)
        {
            return;
        }

        BiomeType[,] nextMap = new BiomeType[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BiomeType current = generatedMap.GetBiome(x, y);
                int sameCount = 0;
                Dictionary<BiomeType, int> counts = new Dictionary<BiomeType, int>();

                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    for (int offsetX = -1; offsetX <= 1; offsetX++)
                    {
                        if (offsetX == 0 && offsetY == 0)
                        {
                            continue;
                        }

                        BiomeType neighbor = generatedMap.GetBiome(x + offsetX, y + offsetY);
                        if (!counts.ContainsKey(neighbor))
                        {
                            counts.Add(neighbor, 1);
                        }
                        else
                        {
                            counts[neighbor]++;
                        }

                        if (neighbor == current)
                        {
                            sameCount++;
                        }
                    }
                }

                if (sameCount >= minimumSameBiomeNeighbors)
                {
                    nextMap[x, y] = current;
                    continue;
                }

                nextMap[x, y] = GetMostCommonBiome(counts, current);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                generatedMap.SetBiome(x, y, nextMap[x, y]);
            }
        }
    }

    BiomeType GetMostCommonBiome(Dictionary<BiomeType, int> counts, BiomeType fallback)
    {
        int highestCount = -1;
        BiomeType selectedBiome = fallback;

        foreach (KeyValuePair<BiomeType, int> pair in counts)
        {
            if (pair.Value <= highestCount)
            {
                continue;
            }

            highestCount = pair.Value;
            selectedBiome = pair.Key;
        }

        return selectedBiome;
    }

    Vector2 CreateOffset(System.Random random)
    {
        return new Vector2(
            random.Next(-100000, 100000),
            random.Next(-100000, 100000));
    }

    void Reset()
    {
        biomeRules = new[]
        {
            new BiomeRule
            {
                biomeType = BiomeType.Ice,
                priority = 100,
                minHeight = 0.55f,
                maxHeight = 1f,
                minMoisture = 0f,
                maxMoisture = 1f,
                minTemperature = 0f,
                maxTemperature = 0.2f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Volcanic,
                priority = 90,
                minHeight = 0.65f,
                maxHeight = 1f,
                minMoisture = 0f,
                maxMoisture = 0.35f,
                minTemperature = 0.8f,
                maxTemperature = 1f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Mangrove,
                priority = 80,
                minHeight = 0.32f,
                maxHeight = 0.5f,
                minMoisture = 0.7f,
                maxMoisture = 1f,
                minTemperature = 0.55f,
                maxTemperature = 1f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Forest,
                priority = 70,
                minHeight = 0.35f,
                maxHeight = 0.85f,
                minMoisture = 0.55f,
                maxMoisture = 1f,
                minTemperature = 0.3f,
                maxTemperature = 0.75f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Tundra,
                priority = 60,
                minHeight = 0.35f,
                maxHeight = 1f,
                minMoisture = 0.2f,
                maxMoisture = 0.7f,
                minTemperature = 0.15f,
                maxTemperature = 0.35f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Desert,
                priority = 50,
                minHeight = 0.35f,
                maxHeight = 0.9f,
                minMoisture = 0f,
                maxMoisture = 0.25f,
                minTemperature = 0.65f,
                maxTemperature = 1f
            },
            new BiomeRule
            {
                biomeType = BiomeType.Grassland,
                priority = 10,
                minHeight = 0.32f,
                maxHeight = 1f,
                minMoisture = 0f,
                maxMoisture = 1f,
                minTemperature = 0f,
                maxTemperature = 1f
            }
        };
    }
}
