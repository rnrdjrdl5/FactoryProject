using System;
using Tables;

// 노이즈로 계산된 값이 어떤 조건일 때 특정 Biome으로 판정되는지 정의한다.
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
