using UnityEngine;

[EntityData(typeof(Team))]
[EntityData(typeof(Bag))]
public class MainStorage : Storage
{
    public static string PrefabPath = $"MainStorage/{typeof(MainStorage)}";
}
