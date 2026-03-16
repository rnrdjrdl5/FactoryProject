using UnityEngine;

[EntityData(typeof(Team))]
[EntityData(typeof(Bag))]
[EntityData(typeof(FactionRelation))]
public class MainStorage : Storage
{
    public static string PrefabPath = $"MainStorage/{typeof(MainStorage)}";

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        // 더미, 추후 수정 필요
        var bag = GetEntityData<Bag>();
        var inventory = bag.GetInventory(Tables.ItemType.Animal);
        var humanItem= inventory.AddItem(Tables.TablesKey.Item_Human, 1);

        var team = GetEntityData<Team>();
        var teamFormation = team.AddTeamFormation();
        
        teamFormation.TryAddPlayer(humanItem);
        humanItem.SetEquip(true);
        
        team.SelectTeamFormation(teamFormation);
    }
}
