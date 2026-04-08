using Tables;
using UnityEngine;

[EntityData(typeof(TeamStorage))]
[EntityData(typeof(PlayerStorage))]
[EntityData(typeof(Bag))]
public class MainStorage : Storage
{
    public static string PrefabPath = $"MainStorage/{typeof(MainStorage)}";

    public override void Ready()
    {
        base.Ready();
        
        // TODO: Dummy data, update later.
        var processorAbility = GetAbility<ProcessorAbility>();
        var mainStorageProcessor = processorAbility.GetProcessor<MainStorageProcessor>();

        var humanItem = Item.Create(Tables.TablesKey.Item_Human, 1);
        mainStorageProcessor.AddPlayer(humanItem);
        var eagleItem =  Item.Create(Tables.TablesKey.Item_Eagle, 1);
        mainStorageProcessor.AddPlayer(eagleItem);
        var snakeItem = Item.Create(Tables.TablesKey.Item_Snake, 1);
        mainStorageProcessor.AddPlayer(snakeItem);
        var dogItem =  Item.Create(Tables.TablesKey.Item_Dog, 1);
        mainStorageProcessor.AddPlayer(dogItem);


        var bag = GetEntityData<Bag>();
        var weaponInventory = bag.GetInventory(ItemType.Weapon);
        var bowItem =  Item.Create(Tables.TablesKey.Item_WoodBow, 1);
        var swordItem =  Item.Create(Tables.TablesKey.Item_WoodSword, 1);
        var staffItem =  Item.Create(Tables.TablesKey.Item_WoodStaff, 1);
        weaponInventory.AddItem(bowItem);
        weaponInventory.AddItem(swordItem);
        weaponInventory.AddItem(staffItem);
        
        
        var teamStorage = GetEntityData<TeamStorage>();
        var teamFormation = teamStorage.AddTeamFormation();
        teamFormation.TryAddPlayer(humanItem);
        teamFormation.TryAddPlayer(eagleItem);
        teamFormation.TryAddPlayer(snakeItem);
        teamFormation.TryAddPlayer(dogItem);
        
        humanItem.SetEquip(true);
        snakeItem.SetEquip(true);
        dogItem.SetEquip(true);
        eagleItem.SetEquip(true);
        
        teamStorage.SelectTeamFormation(teamFormation);
    }
}
