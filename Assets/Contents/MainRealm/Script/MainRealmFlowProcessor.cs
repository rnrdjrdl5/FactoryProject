using UnityEngine;

public class MainRealmFlowProcessor : Processor
{
    FlowRunnerAbility flowAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        flowAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();

        flowAbility.SetRootProcessorFlow<MainRealmFlow>(this);
    }
}

class MainRealmFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<LoadFlow>(Processor);
        AddChild<IngameFlow>(Processor);
    }
}

class LoadFlow : ProcessorFlow
{
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();
        
        var gameData = Realm.LoadResources<TextAsset>("Core/GameData");
        DataLoader.LoadAllData(gameData.bytes);
        SkillParamTableInitializer.Initialize();
        BuffParamTableInitializer.Initialize();
        Tables.EnumLogic.CachingTable();
        
        Entity.AddEntity<MainStorage>(MainStorage.PrefabPath);
        Entity.AddEntity<InputRealm>(InputRealm.PrefabPath);
        Entity.AddEntity<GlobalRealm>(GlobalRealm.PrefabPath);
    }

    public override void OnUpdateFlow()
    {
        if (Entity.IsReady)
        {
            Finish();
        }
    }
}

class IngameFlow : ProcessorFlow
{
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var context = Realm.GetProcessorContext<MainRealmProcessorContext>();
        context?.MainRealmPlayerEntityProcessor?.CreateControlledHeroTeam();

        // var panelAbility = Processor.Realm.GetAbility<PanelAbility>();
        // var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        // var uiInventoryPanelElement = inventoryPopup.GetPanelElement<UIInventoryPanelElement>();
        // uiInventoryPanelElement.SetItemType(ItemType.Weapon);
        //
        // var entity = brain.Controll as Entity;
        // var bag = entity.GetEntityData<Bag>();
        // bag.AddItem(TablesKey.Item_WoodBow, 1);
        // bag.AddItem(TablesKey.Item_WoodStaff, 1);
        // bag.AddItem(TablesKey.Item_WoodSword, 1);
        //
        // inventoryPopup.SetTargetPanelDatas(entity.ToData());
    }
}
