using UnityEngine;

public class InventoryPopupInputLayerProcessor : BaseInputLayerProcessor
{
    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.StateType == InputStateType.Pressed &&
            (input.KeyCode == KeyCode.I || input.KeyCode == KeyCode.Escape))
        {
            (Entity as Panel)?.Close();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }
}
