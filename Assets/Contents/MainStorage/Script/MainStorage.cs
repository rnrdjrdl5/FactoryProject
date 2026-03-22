using UnityEngine;

[EntityData(typeof(Team))]
[EntityData(typeof(PlayerStorage))]
public class MainStorage : Storage
{
    public static string PrefabPath = $"MainStorage/{typeof(MainStorage)}";

    public override void Ready()
    {
        base.Ready();
        
        // 더미, 추후 수정 필요
        var processorAbility = GetAbility<ProcessorAbility>();
        var mainStorageProcessor = processorAbility.GetProcessor<MainStorageProcessor>();

        var humanItem = Item.Create(Tables.TablesKey.Item_Human, 1);
        mainStorageProcessor.AddPlayerStorage(humanItem);
        var eagleItem =  Item.Create(Tables.TablesKey.Item_Eagle, 1);
        mainStorageProcessor.AddPlayerStorage(eagleItem);
        var snakeItem = Item.Create(Tables.TablesKey.Item_Snake, 1);
        mainStorageProcessor.AddPlayerStorage(snakeItem);
        var dogItem =  Item.Create(Tables.TablesKey.Item_Dog, 1);
        mainStorageProcessor.AddPlayerStorage(dogItem);
        
        var team = GetEntityData<Team>();
        var teamFormation = team.AddTeamFormation();
        teamFormation.TryAddPlayer(humanItem);
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
