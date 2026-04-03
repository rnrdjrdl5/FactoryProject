using UnityEngine;

[EntityData(typeof(GlobalInputBindingData))]
[EntityData(typeof(PhysicalInputBindingData))]
public class InputRealm : Realm
{
    public static string PrefabPath = $"InputRealm/{typeof(InputRealm)}";
}
