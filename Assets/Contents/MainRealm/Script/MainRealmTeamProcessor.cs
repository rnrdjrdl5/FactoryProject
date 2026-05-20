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

    public Team CreateTeam(TeamType teamType, Entity source, long sourceUniqueId = 0)
    {
        var team = Team.Create(teamType, source, sourceUniqueId);
        teams.Add(team);
        return team;
    }

    public Team CreateControlledTeam(Entity source, long sourceUniqueId = 0)
    {
        controlledTeam = CreateTeam(TeamType.PlayerInput, source, sourceUniqueId);
        return controlledTeam;
    }

    public bool TryGetTeam(TeamType teamType, long sourceUniqueId, out Team team)
    {
        team = null;
        if (sourceUniqueId == 0)
        {
            return false;
        }

        team = teams.FirstOrDefault(team =>
            team.TeamType == teamType &&
            team.SourceUniqueId == sourceUniqueId);
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

    public void RemovePlayerFromTeams(Player player)
    {
        if (player == null)
        {
            return;
        }

        controlledTeam?.RemovePlayer(player);

        foreach (var team in teams)
        {
            team.RemovePlayer(player);
        }
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
