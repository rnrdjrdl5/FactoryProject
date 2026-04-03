using UnityEngine;

[EntityData(typeof(GlobalInputBindingData))]
public class GlobalRealm : Realm
{
    public static string PrefabPath = $"GlobalRealm/{typeof(GlobalRealm)}";
}
