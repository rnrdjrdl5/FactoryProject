using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

// 셀과 클러스터의 연결 관계를 저장하고 조회하는 데이터이다.
[Serializable]
public class BiomeClusterMapData
{
    static readonly IReadOnlyList<Vector2Int> EmptyCells = new Vector2Int[0];

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int[] clusterIds;
    [SerializeField] List<BiomeCluster> clusters;

    public int Width => width;
    public int Height => height;
    public int ClusterCount => clusters != null ? clusters.Count : 0;
    public IReadOnlyList<BiomeCluster> Clusters => clusters;

    public BiomeClusterMapData(int width, int height, int[] clusterIds, List<BiomeCluster> clusters)
    {
        this.width = width;
        this.height = height;
        this.clusterIds = clusterIds ?? new int[width * height];
        this.clusters = clusters ?? new List<BiomeCluster>();
    }

    public bool IsInside(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public int GetClusterId(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return -1;
        }

        return clusterIds[ToIndex(x, y)];
    }

    public bool TryGetCluster(int x, int y, out BiomeCluster cluster)
    {
        int clusterId = GetClusterId(x, y);
        return TryGetCluster(clusterId, out cluster);
    }

    public bool TryGetCluster(int clusterId, out BiomeCluster cluster)
    {
        if (clusterId < 0 || clusters == null || clusterId >= clusters.Count)
        {
            cluster = null;
            return false;
        }

        cluster = clusters[clusterId];
        return cluster != null;
    }

    public IReadOnlyList<Vector2Int> GetCells(int clusterId)
    {
        if (!TryGetCluster(clusterId, out BiomeCluster cluster) || cluster.Cells == null)
        {
            return EmptyCells;
        }

        return cluster.Cells;
    }

    int ToIndex(int x, int y)
    {
        return y * width + x;
    }
}
