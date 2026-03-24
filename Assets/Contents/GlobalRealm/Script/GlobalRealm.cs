using UnityEngine;

public class GlobalRealm : Realm
{
    public static string PrefabPath = $"GlobalRealm/{typeof(GlobalRealm)}";

    GlobalProcessor globalProcessor;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = GetAbility<ProcessorAbility>();
        globalProcessor = processorAbility.GetProcessor<GlobalProcessor>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            globalProcessor.OpenTeam();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            globalProcessor.OpenEquipment();
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            globalProcessor?.OpenInventory();
        }
    }
}
