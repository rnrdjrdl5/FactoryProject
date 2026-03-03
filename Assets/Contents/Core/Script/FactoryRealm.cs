using UnityEngine;

public class FactoryRealm : Realm
{
    [SerializeField] string prefabPath = "Core/MainRealm";
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        AddEntity<MainRealm>(prefabPath);
    }
}
