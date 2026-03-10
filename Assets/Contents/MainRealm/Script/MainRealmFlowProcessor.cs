using Tables;
using UnityEngine;

public class MainRealmFlowProcessor : Processor
{
    FlowRunnerAbility flowAbility;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        flowAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();

        var mainRealmFlow = Flow.Create<MainRealmFlow>(ProcessorAbility.Entity);
        mainRealmFlow.SetProcessor(this);
        flowAbility.SetRootFlow(mainRealmFlow);
        flowAbility.Flow.NextChildFlow();
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
        
        Finish();
    }
}

class IngameFlow : ProcessorFlow
{
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var brain = BrainLogic.CreateBrainAndEntity(Processor.Realm, Brain.PrefabPath, Player.PrefabPath);
        Processor.Realm.AddEntity<Spawner>(Spawner.PrefabName);

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
