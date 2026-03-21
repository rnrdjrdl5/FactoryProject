using UnityEngine;

public class FactoryRealm : Realm
{
    [SerializeField] string prefabPath = "Core/MainRealm";
    
    protected override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        AddEntity<MainRealm>(prefabPath);
    }
}
