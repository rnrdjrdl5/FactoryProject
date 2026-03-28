public static class SkillParamTableInitializer
{
    public static void Initialize()
    {
        Reset();
        Build();
    }

    public static void Build()
    {
        foreach (var skill in Tables.Skill.Table.Values)
        {
            skill.BuildParsedParams();
        }
    }

    public static void Reset()
    {
        foreach (var skill in Tables.Skill.Table.Values)
        {
            skill.ClearParsedParams();
        }
    }
}
