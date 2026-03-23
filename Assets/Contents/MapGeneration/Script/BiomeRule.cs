using System;
using Tables;

// 노이즈 조건에 따라 특정 바이옴으로 판정하는 규칙이다.
[Serializable]
public class BiomeRule
{
    public BiomeType biomeType = BiomeType.Grassland;
    public int priority;

    public float minHeight = 0.3f;
    public float maxHeight = 1f;

    public float minMoisture = 0f;
    public float maxMoisture = 1f;

    public float minTemperature = 0f;
    public float maxTemperature = 1f;

    public bool Matches(float height, float moisture, float temperature)
    {
        return height >= minHeight && height <= maxHeight
            && moisture >= minMoisture && moisture <= maxMoisture
            && temperature >= minTemperature && temperature <= maxTemperature;
    }
}
