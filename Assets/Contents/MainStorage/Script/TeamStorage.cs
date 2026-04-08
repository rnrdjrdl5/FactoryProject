using System.Collections.Generic;
using System.Linq;

public class TeamStorage : IEntityData, IMessageBus
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
    public IReadOnlyList<TeamFormationStorage> TeamFormations => teamFormations;
    public TeamFormationStorage SelectedTeamFormation => selectedFormation;
    
    List<TeamFormationStorage> teamFormations = new();
    TeamFormationStorage selectedFormation;
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
    }

    public TeamFormationStorage AddTeamFormation(string formationName)
    {
        var teamFormation = TeamFormationStorage.Create(formationName);
        teamFormation.MessageBus = MessageBus;
        teamFormation.OnSetMessageBus();
        
        teamFormations.Add(teamFormation);
        
        MessageBus?.Publish(new EntityDataMsg.TeamFormationAddedMsg
        {
            TeamStorage = this,
            Formation = teamFormation
        });

        return teamFormation;
    }

    public TeamFormationStorage AddTeamFormation()
    {
        var nextCount = teamFormations.Count + 1;
        return AddTeamFormation($"{FormationDefaultName} {nextCount}");
    }

    public bool TryRemoveTeamFormation(TeamFormationStorage teamFormation)
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
                TeamStorage = this,
                Formation = null
            });
        }

        MessageBus?.Publish(new EntityDataMsg.TeamFormationRemovedMsg
        {
            TeamStorage = this,
            Formation = teamFormation
        });
        return true;
    }

    public void SelectTeamFormation(TeamFormationStorage teamFormation)
    {
        if (selectedFormation == teamFormation)
        {
            return;
        }

        selectedFormation = teamFormation;

        MessageBus?.Publish(new EntityDataMsg.TeamSelectedFormationChangedMsg
        {
            TeamStorage = this,
            Formation = teamFormation
        });
    }
}

public static partial class EntityDataMsg
{
    public struct TeamFormationAddedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public TeamStorage TeamStorage;
        public TeamFormationStorage Formation;
    }

    public struct TeamFormationRemovedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public TeamStorage TeamStorage;
        public TeamFormationStorage Formation;
    }

    public struct TeamSelectedFormationChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public TeamStorage TeamStorage;
        public TeamFormationStorage Formation;
    }
}
