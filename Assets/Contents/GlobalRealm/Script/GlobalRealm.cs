using UnityEngine;

[EntityData(typeof(GlobalInputBindingData))]
[EntityData(typeof(PhysicalInputBindingData))]
public class GlobalRealm : Realm
{
    public static string PrefabPath = $"GlobalRealm/{typeof(GlobalRealm)}";
}
