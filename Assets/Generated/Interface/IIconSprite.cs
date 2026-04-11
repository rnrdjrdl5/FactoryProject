using UnityEngine;

public interface IIconSprite
{
    string iconSpritePath { get; set; }
    
    public Sprite GetIconSprite() => Realm.LoadImage(iconSpritePath);
}