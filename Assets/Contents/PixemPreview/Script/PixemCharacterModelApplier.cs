using Tables;

public static class PixemCharacterModelApplier
{
    public static void Apply(PlayerData playerData, PixemRuntimeCharacter character)
    {
        if (playerData == null || character == null)
        {
            return;
        }

        character.ResetAllParts();
        ApplyBaseSkin(playerData, character);
        ApplyEquipments(playerData, character);
        character.SyncAllSprites();
    }

    public static void ApplyBaseSkin(PlayerData playerData, PixemRuntimeCharacter character)
    {
        if (playerData == null || character == null)
        {
            return;
        }

        var tableData = playerData.TableData;
        if (tableData == null)
        {
            return;
        }

        var skinData = Skin.Get(tableData.skinKey);
        if (skinData == null)
        {
            return;
        }

        character.EquipPart(PixemPartType.Body, skinData.bodyKey);
        character.EquipPart(PixemPartType.FaceAcc1, skinData.eyeKey);
        character.EquipPart(PixemPartType.Hair, skinData.hairKey);
    }

    public static void ApplyEquipments(PlayerData playerData, PixemRuntimeCharacter character)
    {
        if (playerData?.Equipment == null || character == null)
        {
            return;
        }

        foreach (var item in playerData.Equipment.EquipItems)
        {
            ApplyEquipmentItem(item, character);
        }
    }

    public static void ApplyEquipmentItem(Item item, PixemRuntimeCharacter character)
    {
        if (item?.ItemData == null || character == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(item.ItemData.equipPath))
        {
            return;
        }

        var pixemPartType = item.ItemData.itemType.ToPixemPartType();
        character.EquipPart(pixemPartType, item.ItemData.equipPath);
    }

    public static void UnequipEquipmentItem(Item item, PixemRuntimeCharacter character)
    {
        if (item?.ItemData == null || character == null)
        {
            return;
        }

        var itemTypes = item.ItemData.itemSlotType.ToItemTypes();
        for (int i = 0; i < itemTypes.Length; i++)
        {
            var pixemPartType = itemTypes[i].ToPixemPartType();
            character.UnequipPart(pixemPartType);
        }
    }
}
