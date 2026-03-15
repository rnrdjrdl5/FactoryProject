using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TeamFormation : IMessageBus
{
    public IReadOnlyList<Item> Players => players;
    public string FormationName => formationName;
    public MessageBus MessageBus { get; set; }
    
    List<Item> players = new();
    string formationName;
    
    public void OnSetMessageBus()
    {
    }
    
    public bool TryAddPlayer(Item item)
    {
        if (!IsPlayerType(item))
        {
            return false;
        }

        players.Add(item);
        MessageBus?.Publish(new EntityDataMsg.TeamFormationChangedMsg
        {
            Formation = this
        });

        return true;
    }

    public bool RemovePlayer(Item item)
    {
        if (!IsPlayerType(item) || !players.Contains(item))
        {
            return false;
        }
        
        players.Remove(item);
        MessageBus?.Publish(new EntityDataMsg.TeamFormationChangedMsg
        {
            Formation = this
        });

        return true;
    }

    bool IsPlayerType(Item item)
    {
        return item.ItemData.itemType == ItemType.Animal;
    }

    public static TeamFormation Create(string formationName)
    {
        var teamFormation = new TeamFormation();
        teamFormation.formationName = formationName;
        return teamFormation;
    }
}

public static partial class EntityDataMsg
{
    public struct TeamFormationChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public TeamFormation Formation;
    }
}
