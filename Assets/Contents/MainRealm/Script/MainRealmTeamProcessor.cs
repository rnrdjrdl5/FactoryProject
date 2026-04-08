using System.Collections.Generic;
using System.Linq;

public class MainRealmTeamProcessor : Processor
{
    public IReadOnlyList<Team> Teams => teams;

    readonly List<Team> teams = new();

    public override void Uninitialize()
    {
        ClearTeams();
        
        base.Uninitialize();
    }

    public Team CreateTeam(TeamType teamType, Entity source)
    {
        var team = Team.Create(teamType, source);
        teams.Add(team);
        return team;
    }

    public bool TryGetTeam(long teamUniqueId, out Team team)
    {
        team = teams.FirstOrDefault(team => team.UniqueId == teamUniqueId);
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

    void ClearTeams()
    {
        foreach (var team in teams)
        {
            team.Clear();
        }

        teams.Clear();
    }
}
