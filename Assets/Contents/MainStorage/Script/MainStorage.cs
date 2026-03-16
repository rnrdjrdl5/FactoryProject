using UnityEngine;

[EntityData(typeof(Team))]
[EntityData(typeof(Bag))]
public class MainStorage : Storage
{
    public static string PrefabPath = $"MainStorage/{typeof(MainStorage)}";

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        // 더미, 추후 수정 필요
        var bag = GetEntityData<Bag>();
        var inventory = bag.GetInventory(Tables.ItemType.Player);
        var humanItem= inventory.AddItem(Tables.TablesKey.Item_Human, 1);

        var team = GetEntityData<Team>();
        var teamFormation = team.AddTeamFormation();
        
        teamFormation.TryAddPlayer(humanItem);
        
        var eagleItem = inventory.AddItem(Tables.TablesKey.Item_Eagle, 1);
        var snakeItem = inventory.AddItem(Tables.TablesKey.Item_Snake, 1);
        var dogItem = inventory.AddItem(Tables.TablesKey.Item_Dog, 1);
        teamFormation.TryAddPlayer(eagleItem);
        teamFormation.TryAddPlayer(snakeItem);
        teamFormation.TryAddPlayer(dogItem);
        
        humanItem.SetEquip(true);
        snakeItem.SetEquip(true);
        dogItem.SetEquip(true);
        eagleItem.SetEquip(true);
        
        team.SelectTeamFormation(teamFormation);
    }
}
