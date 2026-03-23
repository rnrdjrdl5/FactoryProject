using System.Collections.Generic;
using Tables;
using UnityEngine;

// 바이옴 맵을 순회하며 연결된 셀들을 클러스터로 묶어낸다.
public static class BiomeClusterBuilder
{
    static readonly Vector2Int[] CardinalDirections =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    static readonly Vector2Int[] DiagonalDirections =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

    public static BiomeClusterMapData Build(BiomeMapData mapData, bool includeDiagonals)
    {
        if (mapData == null)
        {
            return new BiomeClusterMapData(0, 0, new int[0], new List<BiomeCluster>());
        }

        int width = mapData.Width;
        int height = mapData.Height;
        int[] clusterIds = new int[width * height];
        for (int i = 0; i < clusterIds.Length; i++)
        {
            clusterIds[i] = -1;
        }

        List<BiomeCluster> clusters = new List<BiomeCluster>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int[] directions = includeDiagonals ? DiagonalDirections : CardinalDirections;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = ToIndex(x, y, width);
                if (clusterIds[index] >= 0)
                {
                    continue;
                }

                BiomeType biomeType = mapData.GetBiome(x, y);
                int clusterId = clusters.Count;
                List<Vector2Int> cells = new List<Vector2Int>();

                queue.Enqueue(new Vector2Int(x, y));
                clusterIds[index] = clusterId;

                while (queue.Count > 0)
                {
                    Vector2Int cell = queue.Dequeue();
                    cells.Add(cell);

                    for (int i = 0; i < directions.Length; i++)
                    {
                        Vector2Int neighbor = cell + directions[i];
                        if (!mapData.IsInside(neighbor.x, neighbor.y))
                        {
                            continue;
                        }

                        int neighborIndex = ToIndex(neighbor.x, neighbor.y, width);
                        if (clusterIds[neighborIndex] >= 0)
                        {
                            continue;
                        }

                        if (mapData.GetBiome(neighbor.x, neighbor.y) != biomeType)
                        {
                            continue;
                        }

                        clusterIds[neighborIndex] = clusterId;
                        queue.Enqueue(neighbor);
                    }
                }

                clusters.Add(new BiomeCluster(clusterId, biomeType, cells));
            }
        }

        return new BiomeClusterMapData(width, height, clusterIds, clusters);
    }

    static int ToIndex(int x, int y, int width)
    {
        return y * width + x;
    }
}
