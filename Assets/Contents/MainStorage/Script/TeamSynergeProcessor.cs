using System.Collections.Generic;

public class TeamSynergeProcessor : Processor
{
    Team team;
    readonly List<ISynerge> activeSynerges = new();

    public IReadOnlyList<ISynerge> ActiveSynerges => activeSynerges;

    public override void Ready()
    {
        base.Ready();

        team = Entity.GetEntityData<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            team.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        RefreshActiveSynerges();
    }

    public override void Uninitialize()
    {
        if (team?.MessageBus != null)
        {
            team.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            team.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (team == null || msg.Formation == null)
        {
            return;
        }

        if (msg.Formation != team.SelectedTeamFormation)
        {
            return;
        }

        RefreshActiveSynerges();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (team == null || msg.Team != team)
        {
            return;
        }

        RefreshActiveSynerges();
    }

    void RefreshActiveSynerges()
    {
        if (team == null)
        {
            return;
        }

        activeSynerges.Clear();
        activeSynerges.AddRange(TeamSynergeLogic.GetAllSynerges(team.SelectedTeamFormation));
    }
}
