using UnityEngine;
using UnityEngine.UI;

public class UIPixemCharacterPreviewPanelElement : PanelElement
{
    [SerializeField] RawImage rawImage;
    [SerializeField] PixemCharacterPreviewRenderer previewRenderer;

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        BindTexture();
    }

    public override void Uninitialize()
    {
        Clear();

        if (rawImage != null)
        {
            rawImage.texture = null;
        }

        base.Uninitialize();
    }

    public void SetPlayerData(PlayerData playerData)
    {
        BindTexture();
        previewRenderer?.SetPlayerData(playerData);
    }

    public void Refresh()
    {
        BindTexture();
        previewRenderer?.Refresh();
    }

    public void Clear()
    {
        previewRenderer?.Clear();
    }

    void BindTexture()
    {
        if (rawImage == null || previewRenderer == null)
        {
            return;
        }

        rawImage.texture = previewRenderer.Texture;
    }
}
