using System.Collections.Generic;
using System.Linq;

public class MainRealmTeamProcessor : Processor
{
    public IReadOnlyList<Team> Teams => teams;
    public Team ControlledTeam => controlledTeam;

    readonly List<Team> teams = new();
    Team controlledTeam;

    public override void Uninitialize()
    {
        ClearControlledTeam();
        ClearTeams();
        
        base.Uninitialize();
    }

    public Team CreateTeam(TeamType teamType, Entity source, long teamFormationUniqueId = 0)
    {
        var team = Team.Create(teamType, source, teamFormationUniqueId);
        teams.Add(team);
        return team;
    }

    public Team CreateControlledTeam(Entity source)
    {
        controlledTeam = CreateTeam(TeamType.PlayerInput, source);
        return controlledTeam;
    }

    public bool TryGetTeam(long teamUniqueId, out Team team)
    {
        team = teams.FirstOrDefault(team => team.UniqueId == teamUniqueId);
        return team != null;
    }

    public bool TryGetTeam(TeamType teamType, long teamFormationUniqueId, out Team team)
    {
        team = null;
        if (teamFormationUniqueId == 0)
        {
            return false;
        }

        team = teams.FirstOrDefault(team =>
            team.TeamType == teamType &&
            team.TeamFormationUniqueId == teamFormationUniqueId);
        return team != null;
    }

    public bool TryRemoveTeam(Team team)
    {
        if (team == null || !teams.Remove(team))
        {
            return false;
        }

        team.Clear();
        return true;
    }

    public bool TryRemoveControlledTeam()
    {
        if (!TryRemoveTeam(controlledTeam))
        {
            return false;
        }

        controlledTeam = null;
        return true;
    }

    void ClearTeams()
    {
        foreach (var team in teams)
        {
            team.Clear();
        }

        teams.Clear();
    }

    void ClearControlledTeam()
    {
        controlledTeam?.Clear();
        controlledTeam = null;
    }
}
