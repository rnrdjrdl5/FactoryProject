public static class PlayerLogic
{
    public static Tables.FactionRelationType GetFactionRelationType(Player mainPlayer, Player targetPlayer)
    {
        return Tables.FactionRelation.GetRelation(mainPlayer.PlayerData.factionType, targetPlayer.PlayerData.factionType);
    }
}