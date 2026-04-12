using System.Collections.Generic;
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
        var bag = GetEntityData<Bag>();
        var testPlayerItems = new List<Item>();

        foreach (var itemTable in Tables.Item.Table.Values)
        {
            var item = Item.Create(itemTable.Key, 1);
            if (item.ItemData.itemSlotType == Tables.ItemSlotType.Player)
            {
                mainStorageProcessor.AddPlayer(item);
                testPlayerItems.Add(item);
                continue;
            }

            bag.AddItem(item);
        }
        
        var teamStorage = GetEntityData<TeamStorage>();
        var teamFormation = teamStorage.AddTeamFormation();
        var testPlayerCount = Mathf.Min(4, testPlayerItems.Count);
        for (int i = 0; i < testPlayerCount; i++)
        {
            var playerItem = testPlayerItems[i];
            teamFormation.TryAddPlayer(playerItem);
            playerItem.SetEquip(true);
        }
        
        teamStorage.SelectTeamFormation(teamFormation);
    }
}
