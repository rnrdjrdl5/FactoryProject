using UnityEngine;

public class GlobalRealm : Realm
{
    public static string PrefabPath = $"GlobalRealm/{typeof(GlobalRealm)}";

    GlobalTeamProcessor globalTeamProcessor;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = GetAbility<ProcessorAbility>();
        globalTeamProcessor = processorAbility.GetProcessor<GlobalTeamProcessor>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            globalTeamProcessor.OpenTeam();
        }
    }
}
