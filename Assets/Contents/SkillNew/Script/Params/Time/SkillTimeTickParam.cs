public class SkillTimeTickParam : ISkillTimeParam
{
    // Positional schema: [intervalSeconds, repeatCount]
    public float? IntervalSeconds { get; set; }
    public int? RepeatCount { get; set; }
}
