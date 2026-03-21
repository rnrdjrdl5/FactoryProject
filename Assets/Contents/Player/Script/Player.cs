using UnityEngine;

[EntityData(typeof(PlayerData))]
public class Player : Entity
{
    public Tables.Player TableData => tableData;
    
    Tables.Player tableData;
    string playerKey;
    PlayerData playerData;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is PlayerInitData playerInitData)
        {
            playerKey = playerInitData.PlayerKey;
            tableData = Tables.Player.Get(playerKey);
            transform.position = playerInitData.Position;
        }
        
        base.Initialize(initData);

        playerData = GetEntityData<PlayerData>();
        AddStat();
    }

    public override void Uninitialize()
    {
        RemoveStat();
        
        base.Uninitialize();
    }

    void AddStat()
    {
        if (playerData?.Stat == null)
        {
            return;
        }
        
        playerData.Stat.AddStats(tableData);
    }

    void RemoveStat()
    {
        if (playerData?.Stat == null)
        {
            return;
        }

        playerData.Stat.RemoveStats(tableData);
    }
}

public class PlayerInitData : IInitData
{
    public string PlayerKey;
    public Vector3 Position;
}
