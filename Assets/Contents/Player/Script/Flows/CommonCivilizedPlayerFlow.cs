public class CommonCivilizedPlayerFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<NoneFlow>(Processor);
    }
}