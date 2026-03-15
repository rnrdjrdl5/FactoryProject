using System;
using System.Collections.Generic;
using System.Linq;

public class Team : IEntityData
{
    public event Action OnChanged;
    public IReadOnlyList<TeamFormation> TeamFormations => teamFormations;
    
    List<TeamFormation> teamFormations = new();
    TeamFormation selectedTeam;
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
    }

    public void AddTeamFormation(string formationName)
    {
        var teamFormation = TeamFormation.Create(formationName);
        teamFormations.Add(teamFormation);
        OnChanged?.Invoke();
    }

    public bool TryRemoveTeamFormation(TeamFormation teamFormation)
    {
        if (!teamFormations.Contains(teamFormation))
        {
            return false;
        }

        teamFormations.Remove(teamFormation);
        OnChanged?.Invoke();
        return true;
    }

    public IEnumerable<Item> GetEquipItemKeys(IEnumerable<Item> items)
    {
        if (items == null || !items.Any())
        {
            return null;
        }

        return teamFormations
            .SelectMany(teamFormation => teamFormation.Players)
            .Where(items.Contains);
    }
}
