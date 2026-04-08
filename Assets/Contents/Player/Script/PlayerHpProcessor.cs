using UnityEngine;

public class PlayerHpProcessor : Processor
{
    Player player;
    PlayerData playerData;
    HpAbility hpAbility;
    DropItemProcessor dropItemProcessor;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        player = Entity as Player;
        playerData = Entity.GetEntityData<PlayerData>();
        
        hpAbility = Entity.GetAbility<HpAbility>();
        var runtimeData = playerData?.RuntimeData;

        hpAbility.SetMaxHp(runtimeData?.MaxHp ?? 0, false);
        hpAbility.SetHp(runtimeData?.Hp ?? 0);
        hpAbility.OnChangeHp += OnChangedHp;
    }

    public override void Uninitialize()
    {
        hpAbility.OnChangeHp -= OnChangedHp;

        playerData = null;
        
        base.Uninitialize();
    }
    
    void OnChangedHp(float prevHp, float hp)
    {
        playerData?.RuntimeData?.SetHp(hp);

        if (hp <= 0)
        {
            if (playerData?.OriginType == PlayerOriginType.WorldSpawned)
            {
                dropItemProcessor ??= ProcessorAbility.GetProcessor<DropItemProcessor>();
                dropItemProcessor.TryDropItem(Entity.transform.position, player.TableData.dropPlayerPercent, player.TableData.dropPlayerKey);
            }
            
            DestroyPlayer();
        }
    }
    
    void DestroyPlayer()
    {
        Realm.RemoveChild(Entity);
    }
}
