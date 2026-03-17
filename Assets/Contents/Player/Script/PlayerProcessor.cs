public class PlayerProcessor : Processor
{
    Player player;
    Faction faction;
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        player = Entity as Player;
        faction = Entity.GetEntityData<Faction>();
        faction.SetFactionType(player.PlayerData.factionType);
    }
}
