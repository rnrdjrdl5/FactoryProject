using UnityEngine;

public class UIStatPanelElement : PanelElement
{
    [SerializeField] AllocGameObject allocGameObject;

    Stat stat;

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        
        RefreshUI();
    }

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();

        var playerData = GetTargetPanelDatas<PlayerData>();
        if (playerData != null)
        {
            stat = playerData.Stat;
            
            stat.MessageBus.Subscribe<EntityDataMsg.StatChangedMsg>(OnStatChanged);
        }

        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        if (stat != null)
        {
            stat.MessageBus.Unsubscribe<EntityDataMsg.StatChangedMsg>(OnStatChanged);
        }

        stat = null;
        
        base.OnUnsetPanelDatas();
    }

    public override void RefreshUI()
    {
        if (stat == null)
        {
            allocGameObject.DeallocateObjects();
            return;
        }
        
        base.RefreshUI();
        
        allocGameObject.DeallocateObjects();
        allocGameObject.AllocateObject(Tables.EnumLogic.StatTypes.Length);
        
        for (int i = 0; i < allocGameObject.AllocatedObjects.Count; i++)
        {
            var allocObject = allocGameObject.AllocatedObjects[i];
            var uiStat = allocObject.GetComponent<UIStat>();
            var statType = Tables.EnumLogic.StatTypes[i];
            uiStat.UpdateStat(statType, !stat.TryGetStat(statType, out var value) ? 0 : value);
        }
    }

    void OnStatChanged(EntityDataMsg.StatChangedMsg msg)
    {
        RefreshUI();
    }
}
