using UnityEngine;

public class TeamPopupInputLayerProcessor : BaseInputLayerProcessor
{
    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.StateType == InputStateType.Pressed &&
            (input.KeyCode == KeyCode.F1 || input.KeyCode == KeyCode.Escape))
        {
            (Entity as Panel)?.Close();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }
}
