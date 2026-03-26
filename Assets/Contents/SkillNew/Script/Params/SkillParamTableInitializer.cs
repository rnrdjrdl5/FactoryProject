public static class SkillParamTableInitializer
{
    static bool isInitialized;

    public static void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        Rebuild();
        isInitialized = true;
    }

    public static void Rebuild()
    {
        foreach (var skill in Tables.Skill.Table.Values)
        {
            skill.BuildParsedParams();
        }
    }

    public static void Reset()
    {
        isInitialized = false;

        foreach (var skill in Tables.Skill.Table.Values)
        {
            skill.ClearParsedParams();
        }
    }
}
