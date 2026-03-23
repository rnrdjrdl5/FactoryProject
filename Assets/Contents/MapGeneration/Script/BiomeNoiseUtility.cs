using UnityEngine;

// 맵 생성에 사용할 부드러운 프랙탈 노이즈 값을 샘플링한다.
public static class BiomeNoiseUtility
{
    public static float SampleFractalNoise(
        int x,
        int y,
        float scale,
        int octaves,
        float persistence,
        float lacunarity,
        Vector2 offset)
    {
        scale = Mathf.Max(0.0001f, scale);
        octaves = Mathf.Max(1, octaves);
        persistence = Mathf.Clamp01(persistence);
        lacunarity = Mathf.Max(1f, lacunarity);

        float amplitude = 1f;
        float frequency = 1f;
        float total = 0f;
        float totalAmplitude = 0f;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (x + offset.x) * scale * frequency;
            float sampleY = (y + offset.y) * scale * frequency;
            float value = Mathf.PerlinNoise(sampleX, sampleY);

            total += value * amplitude;
            totalAmplitude += amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        if (totalAmplitude <= 0f)
        {
            return 0f;
        }

        return total / totalAmplitude;
    }
}
