public static class FactionLogic
{
    public static bool IsHostile(Player mainPlayer, Player targetPlayer)
    {
        return GetFactionRelationType(mainPlayer, targetPlayer) == Tables.FactionRelationType.Hostile;
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
