using System;
using System.Collections.Generic;
using System.Linq;

public class Team : IEntityData
{
    const string FormationDefaultName = "Formation";
    
    public event Action OnChanged;
    public IReadOnlyList<TeamFormation> TeamFormations => teamFormations;
    public TeamFormation SelectedTeamFormation => selectedFormation;
    
    List<TeamFormation> teamFormations = new();
    TeamFormation selectedFormation;
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
    }

    public TeamFormation AddTeamFormation(string formationName)
    {
        var teamFormation = TeamFormation.Create(formationName);
        teamFormations.Add(teamFormation);
        OnChanged?.Invoke();

        return teamFormation;
    }

    public TeamFormation AddTeamFormation()
    {
        var nextCount = teamFormations.Count + 1;
        return AddTeamFormation($"{FormationDefaultName} {nextCount}");
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

    public void SelectTeamFormation(TeamFormation teamFormation)
    {
        selectedFormation = teamFormation;
        OnChanged?.Invoke();
    }
}
