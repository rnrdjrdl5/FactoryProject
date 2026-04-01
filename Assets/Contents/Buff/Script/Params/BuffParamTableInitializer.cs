public static class BuffParamTableInitializer
{
    public static void Initialize()
    {
        Reset();
        Build();
    }

    public static void Build()
    {
        foreach (var buff in Tables.Buff.Table.Values)
        {
            buff.BuildParsedParams();
        }
    }

    public static void Reset()
    {
        foreach (var buff in Tables.Buff.Table.Values)
        {
            buff.ClearParsedParams();
        }
    }
}
