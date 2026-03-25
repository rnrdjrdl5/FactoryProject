using Tables;

public static class DamageLogic
{
    public const int DefaultDamage = 5;

    public static int GetDamage(Stat stat)
    {
        if (stat == null)
        {
            return DefaultDamage;
        }

        if (stat.TryGetStat(StatType.Physical, out var physicalDamage))
        {
            return physicalDamage;
        }

        return DefaultDamage;
    }

    public static int GetDamage(PlayerData playerData)
    {
        return GetDamage(playerData?.Stat);
    }
}
