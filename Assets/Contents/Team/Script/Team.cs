using System.Collections.Generic;
using System.Linq;

public class Team
{
    readonly List<Player> players = new();

    public long UniqueId { get; private set; }
    public TeamType TeamType { get; private set; }
    public Entity Source { get; private set; }
    public IReadOnlyList<Player> Players => players;
    public Player Leader => Players.FirstOrDefault();

    public bool ContainsPlayer(Player player)
    {
        return player != null && players.Contains(player);
    }

    public bool TryAddPlayer(Player player)
    {
        if (player == null || ContainsPlayer(player))
        {
            return false;
        }

        players.Add(player);
        return true;
    }

    public bool RemovePlayer(Player player)
    {
        if (!ContainsPlayer(player))
        {
            return false;
        }

        return players.Remove(player);
    }

    public void Clear()
    {
        players.Clear();
    }

    public static Team Create(TeamType teamType, Entity source)
    {
        var team = new Team();
        team.UniqueId = IDLogic.NewUniqueId();
        team.TeamType = teamType;
        team.Source = source;
        return team;
    }
}
