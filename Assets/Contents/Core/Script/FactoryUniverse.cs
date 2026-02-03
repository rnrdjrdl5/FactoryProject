using UnityEngine;

public class FactoryUniverse : Universe
{
    protected override string[] GetModule()
    {
        return new[]
        {
            nameof(ObjectPoolModule),
            nameof(StageModule),
            nameof(LocalDataModule),
        };
    }

    protected override void Initialize()
    {
        base.Initialize();

        var stageModule = Environment.GetModule<StageModule>();
        stageModule.AddStage<FactoryStage>("Core/FactoryStage");
    }
}
