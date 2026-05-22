[Processor(typeof(MainStorageProcessor))]
[Processor(typeof(MainStorageSynergeProcessor))]
public class MainStorageProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<MainStorageProcessorContext>();
    }
}
