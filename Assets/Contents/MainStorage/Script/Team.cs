using System;
using System.Collections.Generic;

// Team -> TeamFormation -> Item(Player)
public class Team : IEntityData
{
    public event Action OnChanged;
    List<TeamFormation> teamFormations = new();
    TeamFormation selectedTeam;
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
    }

    public void AddTeamFormation()
    {
        var teamFormation = TeamFormation.Create();
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
}