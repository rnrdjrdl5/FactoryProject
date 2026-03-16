using System.Collections.Generic;
using System.Linq;
using Tables;
using UnityEngine;

public class TeamFormation : IMessageBus
{
    public IReadOnlyList<Item> Players => players;
    public string FormationName => formationName;
    public MessageBus MessageBus { get; set; }
    public Item Leader => leader; 
    
    List<Item> players = new();
    Item leader;
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
        UpdateLeader();
        
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
        UpdateLeader();
        
        MessageBus?.Publish(new EntityDataMsg.TeamFormationChangedMsg
        {
            Formation = this
        });

        return true;
    }

    bool IsPlayerType(Item item)
    {
        return item.ItemData.itemType == ItemType.Player;
    }

    void UpdateLeader()
    {
        var nextLeader = players.FirstOrDefault();
        if (leader != null && leader == nextLeader)
        {
            return;
        }

        leader = nextLeader;
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
