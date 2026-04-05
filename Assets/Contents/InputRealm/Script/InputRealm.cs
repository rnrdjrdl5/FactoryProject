using UnityEngine;

[EntityData(typeof(GlobalInputBindingData))]
public class InputRealm : FrameworkInputRealm
{
    public static string PrefabPath = $"InputRealm/{typeof(InputRealm)}";
}
