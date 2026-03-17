public class Faction : IEntityData
{
    public Tables.FactionType FactionType => factionType;
    
    Tables.FactionType factionType;
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    public void SetFactionType(Tables.FactionType factionType)
    {
        this.factionType = factionType;
    }
}