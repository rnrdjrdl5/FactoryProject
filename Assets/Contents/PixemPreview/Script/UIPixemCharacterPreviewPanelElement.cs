using UnityEngine;
using UnityEngine.UI;

public class UIPixemCharacterPreviewPanelElement : PanelElement
{
    [SerializeField] RawImage rawImage;

    public override void Uninitialize()
    {
        Clear();

        base.Uninitialize();
    }

    public void SetTexture(Texture texture)
    {
        if (rawImage == null)
        {
            return;
        }

        rawImage.texture = texture;
    }

    public void Clear()
    {
        SetTexture(null);
    }
}
