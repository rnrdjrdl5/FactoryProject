using System.Collections.Generic;
using System.Linq;

public class Team : IEntityData, IMessageBus
{
    const string FormationDefaultName = "Formation";

    public MessageBus MessageBus { get; set; }

    public void OnSetMessageBus()
    {
        if (MessageBus == null)
            return;

        foreach (var formation in teamFormations)
        {
            formation.MessageBus = MessageBus;
            formation.OnSetMessageBus();
        }
    }
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
        teamFormation.MessageBus = MessageBus;
        teamFormation.OnSetMessageBus();
        
        teamFormations.Add(teamFormation);
        MessageBus?.Publish(new TeamFormationAddedMsg
        {
            Team = this,
            Formation = teamFormation
        });

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
        if (selectedFormation == teamFormation)
        {
            selectedFormation = null;
            MessageBus?.Publish(new TeamSelectedFormationChangedMsg
            {
                Team = this,
                Formation = null
            });
        }

        MessageBus?.Publish(new TeamFormationRemovedMsg
        {
            Team = this,
            Formation = teamFormation
        });
        return true;
    }

    public void SelectTeamFormation(TeamFormation teamFormation)
    {
        if (selectedFormation == teamFormation)
        {
            return;
        }

        selectedFormation = teamFormation;

        MessageBus?.Publish(new TeamSelectedFormationChangedMsg
        {
            Team = this,
            Formation = teamFormation
        });
    }
}

public struct TeamFormationAddedMsg
{
    public Team Team;
    public TeamFormation Formation;
}

public struct TeamFormationRemovedMsg
{
    public Team Team;
    public TeamFormation Formation;
}

public struct TeamSelectedFormationChangedMsg
{
    public Team Team;
    public TeamFormation Formation;
}
