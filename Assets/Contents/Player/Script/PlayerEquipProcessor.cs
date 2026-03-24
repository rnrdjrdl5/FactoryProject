public class PlayerEquipProcessor : Processor
{
    Equipment equipment;
    Stat stat;
    PlayerData playerData;
    
    public override void Ready()
    {
        base.Ready();

        playerData = Entity.GetEntityData<PlayerData>();
        stat = playerData?.Stat;
        equipment = playerData?.Equipment;
    }
}