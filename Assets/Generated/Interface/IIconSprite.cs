using UnityEngine;

public interface IIconSprite
{
    string iconAtlasPath { get; set; }
    string iconSpritePath { get; set; }

    public Sprite GetIconSprite() => Realm.LoadImageFromAtlas(iconAtlasPath, iconSpritePath);
}
