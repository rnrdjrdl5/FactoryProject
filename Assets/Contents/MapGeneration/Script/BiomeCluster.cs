using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

// 같은 바이옴으로 연결된 셀 묶음 하나를 표현하는 데이터이다.
[Serializable]
public class BiomeCluster
{
    [SerializeField] int id;
    [SerializeField] BiomeType biomeType;
    [SerializeField] List<Vector2Int> cells;
    [SerializeField] RectInt bounds;

    public int Id => id;
    public BiomeType BiomeType => biomeType;
    public IReadOnlyList<Vector2Int> Cells => cells;
    public RectInt Bounds => bounds;
    public int Count => cells != null ? cells.Count : 0;

    public BiomeCluster(int id, BiomeType biomeType, List<Vector2Int> cells)
    {
        this.id = id;
        this.biomeType = biomeType;
        this.cells = cells ?? new List<Vector2Int>();
        bounds = BuildBounds(this.cells);
    }

    static RectInt BuildBounds(List<Vector2Int> cells)
    {
        if (cells == null || cells.Count == 0)
        {
            return new RectInt();
        }

        int minX = cells[0].x;
        int maxX = cells[0].x;
        int minY = cells[0].y;
        int maxY = cells[0].y;

        for (int i = 1; i < cells.Count; i++)
        {
            Vector2Int cell = cells[i];
            minX = Mathf.Min(minX, cell.x);
            maxX = Mathf.Max(maxX, cell.x);
            minY = Mathf.Min(minY, cell.y);
            maxY = Mathf.Max(maxY, cell.y);
        }

        return new RectInt(minX, minY, (maxX - minX) + 1, (maxY - minY) + 1);
    }
}
