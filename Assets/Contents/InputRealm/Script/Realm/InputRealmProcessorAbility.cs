using UnityEngine;

public class InputRealmProcessorAbility : InputProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetContext<InputRealmProcessorContext>();
    }
}
