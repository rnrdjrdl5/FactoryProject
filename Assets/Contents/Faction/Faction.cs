using UnityEngine;

public class Faction : IEntityData
{
    public FactionType FactionType => factionType;
    
    FactionType factionType;
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }
}
