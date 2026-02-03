using UnityEngine;

public class FactoryEntry : Entry
{
    protected override void Initialize()
    {
        base.Initialize();
        
        AddUniverse(Universe.Create<FactoryUniverse>());
    }
}
