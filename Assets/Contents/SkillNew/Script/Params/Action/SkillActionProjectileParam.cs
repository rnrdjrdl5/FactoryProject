public class SkillActionProjectileParam : ISkillActionParam
{
    // Positional schema: [prefabPath, speed, duration]
    public string PrefabPath { get; set; }
    public float? Speed { get; set; }
    public float? Duration { get; set; }
}
