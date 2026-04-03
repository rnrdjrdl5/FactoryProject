using UnityEngine;

[EntityData(typeof(GlobalInputBindingData))]
[EntityData(typeof(PhysicalInputBindingData))]
[EntityData(typeof(PhysicalInputStateData))]
public class InputRealm : Realm
{
    public static string PrefabPath = $"InputRealm/{typeof(InputRealm)}";
}
