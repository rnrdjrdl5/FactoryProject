using UnityEngine;

public class EquipmentPopupInputLayerProcessor : BaseInputLayerProcessor
{
    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.StateType == InputStateType.Pressed &&
            (input.KeyCode == KeyCode.F2 || input.KeyCode == KeyCode.Escape))
        {
            (Entity as Panel)?.Close();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }
}
