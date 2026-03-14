using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TeamFormation
{
    public event Action<Item> OnAddPlayer;
    public event Action<Item> OnRemovePlayer;
    
    List<Item> players = new();
    
    public bool TryAddPlayer(Item item)
    {
        if (!IsPlayerType(item))
        {
            return false;
        }

        players.Add(item);
        OnAddPlayer?.Invoke(item);

        return true;
    }

    public bool RemovePlayer(Item item)
    {
        if (!IsPlayerType(item) || !players.Contains(item))
        {
            return false;
        }
        
        players.Remove(item);
        OnRemovePlayer?.Invoke(item);

        return true;
    }

    bool IsPlayerType(Item item)
    {
        return item.ItemData.itemType == ItemType.Animal;
    }

    public static TeamFormation Create()
    {
        var teamFormation = new TeamFormation();
        return teamFormation;
    }
}
