using UnityEngine;

[EntityData(typeof(Bag))]
[EntityData(typeof(Faction))]
[EntityData(typeof(Stat))]
[EntityData(typeof(Equipment))]
public class Player : Entity
{
    public Tables.Player PlayerData => playerData;
    
    Tables.Player playerData;
    string playerKey;
    Stat stat;

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

        stat = GetEntityData<Stat>();
        AddStat();
    }

    public override void Uninitialize()
    {
        RemoveStat();
        
        base.Uninitialize();
    }

    void AddStat()
    {
        if (stat == null)
        {
            return;
        }
        
        stat.AddStats(playerData);
    }

    void RemoveStat()
    {
        if (stat == null)
        {
            return;
        }

        stat.RemoveStats(playerData);
    }
}

public class PlayerInitData : IInitData
{
    public string PlayerKey;
    public Vector3 Position;
}