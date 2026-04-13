using UnityEngine;

public class PixemCharacterPreviewRenderer : MonoBehaviour
{
    [SerializeField] Camera previewCamera;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] PixemRuntimeCharacter character;
    [SerializeField] bool renderOnEnable;

    PlayerData targetPlayerData;

    public RenderTexture Texture => renderTexture;
    public PlayerData TargetPlayerData => targetPlayerData;

    void OnEnable()
    {
        Subscribe();

        if (renderOnEnable)
        {
            Refresh();
        }
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    public void SetPlayerData(PlayerData playerData)
    {
        if (targetPlayerData == playerData)
        {
            Refresh();
            return;
        }

        Unsubscribe();

        targetPlayerData = playerData;

        Subscribe();
        Refresh();
    }

    public void Clear()
    {
        Unsubscribe();
        targetPlayerData = null;
        ClearRenderTexture();
    }

    public void Refresh()
    {
        if (targetPlayerData == null || character == null)
        {
            ClearRenderTexture();
            return;
        }

        PixemCharacterModelApplier.Apply(targetPlayerData, character);
        RenderOnce();
    }

    public void RenderOnce()
    {
        if (previewCamera == null || renderTexture == null)
        {
            return;
        }

        previewCamera.enabled = false;
        previewCamera.targetTexture = renderTexture;
        previewCamera.Render();
    }

    void Subscribe()
    {
        if (targetPlayerData?.Equipment?.MessageBus == null)
        {
            return;
        }

        targetPlayerData.Equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
        targetPlayerData.Equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
    }

    void Unsubscribe()
    {
        if (targetPlayerData?.Equipment?.MessageBus == null)
        {
            return;
        }

        targetPlayerData.Equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
        targetPlayerData.Equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
    }

    void OnEquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        if (msg.Equipment != targetPlayerData?.Equipment)
        {
            return;
        }

        Refresh();
    }

    void OnUnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        if (msg.Equipment != targetPlayerData?.Equipment)
        {
            return;
        }

        Refresh();
    }

    void ClearRenderTexture()
    {
        if (renderTexture == null)
        {
            return;
        }

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = previous;
    }
}
