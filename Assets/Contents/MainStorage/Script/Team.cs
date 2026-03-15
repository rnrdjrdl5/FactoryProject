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
        MessageBus?.Publish(new EntityDataMsg.TeamFormationAddedMsg
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
            MessageBus?.Publish(new EntityDataMsg.TeamSelectedFormationChangedMsg
            {
                Team = this,
                Formation = null
            });
        }

        MessageBus?.Publish(new EntityDataMsg.TeamFormationRemovedMsg
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

        MessageBus?.Publish(new EntityDataMsg.TeamSelectedFormationChangedMsg
        {
            Team = this,
            Formation = teamFormation
        });
    }
}

public static partial class EntityDataMsg
{
    public struct TeamFormationAddedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Team Team;
        public TeamFormation Formation;
    }

    public struct TeamFormationRemovedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Team Team;
        public TeamFormation Formation;
    }

    public struct TeamSelectedFormationChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Team Team;
        public TeamFormation Formation;
    }
}
