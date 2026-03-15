using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TeamFormation
{
    public event Action OnChanged;
    public IReadOnlyList<Item> Players => players;
    public string FormationName => formationName;
    
    List<Item> players = new();
    string formationName;
    
    public bool TryAddPlayer(Item item)
    {
        if (!IsPlayerType(item))
        {
            return false;
        }

        players.Add(item);
        OnChanged?.Invoke();

        return true;
    }

    public bool RemovePlayer(Item item)
    {
        if (!IsPlayerType(item) || !players.Contains(item))
        {
            return false;
        }
        
        players.Remove(item);
        OnChanged?.Invoke();

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
