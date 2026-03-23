using System;
using Tables;
using UnityEngine;

// 생성된 맵의 셀별 Biome 결과를 저장하고 좌표 조회 기능을 제공한다.
[Serializable]
public class BiomeMapData
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int seed;
    [SerializeField] BiomeType fallbackBiome;
    [SerializeField] BiomeType[] biomeCells;

    public int Width => width;
    public int Height => height;
    public int Seed => seed;

    public BiomeMapData(int width, int height, int seed, BiomeType fallbackBiome)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.fallbackBiome = fallbackBiome;
        biomeCells = new BiomeType[width * height];

        for (int i = 0; i < biomeCells.Length; i++)
        {
            biomeCells[i] = fallbackBiome;
        }
    }

    public bool IsInside(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public BiomeType GetBiome(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return fallbackBiome;
        }

        return biomeCells[ToIndex(x, y)];
    }

    public void SetBiome(int x, int y, BiomeType biome)
    {
        if (!IsInside(x, y))
        {
            return;
        }

        biomeCells[ToIndex(x, y)] = biome;
    }

    public Vector2Int ToCell(Vector2 localPosition, float cellSize)
    {
        if (cellSize <= 0f)
        {
            return Vector2Int.zero;
        }

        return new Vector2Int(
            Mathf.FloorToInt(localPosition.x / cellSize),
            Mathf.FloorToInt(localPosition.y / cellSize));
    }

    int ToIndex(int x, int y)
    {
        return y * width + x;
    }
}
