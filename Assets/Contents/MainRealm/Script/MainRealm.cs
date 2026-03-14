using UnityEngine;

public class MainRealm : Realm
{
    public override void Ready()
    {
        base.Ready();
        
        AddEntity<MainStorage>(MainStorage.PrefabPath);
        AddEntity<GlobalRealm>(GlobalRealm.PrefabPath);
    }
}
