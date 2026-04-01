public enum StatSourceType
{
    None = 0,
    Player = 1,
    Equipment = 2,
    Skill = 3,
}

public readonly struct StatSourceKey : IEquatable<StatSourceKey>
{
    public StatSourceType SourceType { get; }
    public string SubKey { get; }

    public StatSourceKey(StatSourceType sourceType, string subKey)
    {
        SourceType = sourceType;
        SubKey = subKey ?? string.Empty;
    }

    public bool Equals(StatSourceKey other)
    {
        return SourceType == other.SourceType && SubKey == other.SubKey;
    }

    public static bool operator ==(StatSourceKey left, StatSourceKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StatSourceKey left, StatSourceKey right)
    {
        return !left.Equals(right);
    }
}
