using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class TeamFormationStorage : IMessageBus, IUniqueId
{
    public long UniqueId
    {
        get => uniqueId;
        set => uniqueId = value;
    }
    public IReadOnlyList<Item> Players => players;
    public string FormationName => formationName;
    public MessageBus MessageBus { get; set; }
    public Item Leader => leader; 
    
    List<Item> players = new();
    Item leader;
    [JsonProperty] long uniqueId;
    string formationName;
    
    public void OnSetMessageBus()
    {
        InitUniqueId();
    }

    void InitUniqueId()
    {
        if (uniqueId == 0)
        {
            uniqueId = IDLogic.NewUniqueId();
        }
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
        return item.ItemData.itemType == Tables.ItemType.Player;
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

    public static TeamFormationStorage Create(string formationName)
    {
        var teamFormation = new TeamFormationStorage();
        teamFormation.InitUniqueId();
        teamFormation.formationName = formationName;
        return teamFormation;
    }
}

public static partial class EntityDataMsg
{
    public struct TeamFormationChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public TeamFormationStorage Formation;
    }
}
