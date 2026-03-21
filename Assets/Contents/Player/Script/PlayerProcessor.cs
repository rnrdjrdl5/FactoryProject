public class PlayerProcessor : Processor
{
    Player player;
    Faction faction;
    PlayerData playerData;
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        player = Entity as Player;
        playerData = Entity.GetEntityData<PlayerData>();
        faction = playerData?.Faction;
        if (faction != null)
        {
            faction.SetFactionType(player.TableData.factionType);
        }
    }
}
