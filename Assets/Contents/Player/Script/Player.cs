using UnityEngine;

[EntityData(typeof(Bag))]
public class Player : Entity
{
    public Tables.Player PlayerData => playerData;
    
    Tables.Player playerData;
    string playerKey;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is PlayerInitData playerInitData)
        {
            playerKey = playerInitData.PlayerKey;
            playerData = Tables.Player.Get(playerKey);
            transform.position = playerInitData.Position;
        }
        
        base.Initialize(initData);
    }
}

public class PlayerInitData : IInitData
{
    public string PlayerKey;
    public Vector3 Position;
}