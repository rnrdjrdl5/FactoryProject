public static class FactionLogic
{
    // NOTE : 제거 필요
    public static bool IsHostile(Player mainPlayer, Player targetPlayer)
    {
        return IsRelation(mainPlayer, targetPlayer, Tables.FactionRelationType.Hostile);
    }

    // NOTE : 제거 필요
    public static bool IsFriendly(Player mainPlayer, Player targetPlayer)
    {
        return IsRelation(mainPlayer, targetPlayer, Tables.FactionRelationType.Friendly);
    }

    public static bool IsRelation(Player mainPlayer, Player targetPlayer, Tables.FactionRelationType relationType)
    {
        return GetFactionRelationType(mainPlayer, targetPlayer) == relationType;
    }

    public static Tables.FactionRelationType GetFactionRelationType(Player mainPlayer, Player targetPlayer)
    {
        var mainFaction = mainPlayer.GetEntityData<PlayerData>()?.Faction;
        var targetFaction = targetPlayer.GetEntityData<PlayerData>()?.Faction;
        if (mainFaction == null || targetFaction == null)
        {
            return Tables.FactionRelationType.Neutral;
        }

        return Tables.FactionRelation.GetRelation(mainFaction.FactionType, targetFaction.FactionType);
    }
}
