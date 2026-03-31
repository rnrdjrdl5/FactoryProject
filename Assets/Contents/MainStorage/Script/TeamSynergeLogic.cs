using System.Collections.Generic;
using Tables;

public static class TeamSynergeLogic
{
    public static List<ISynerge> GetAllSynerges(TeamFormation teamFormation)
    {
        var synerges = new List<ISynerge>();
        if (teamFormation == null)
        {
            return synerges;
        }

        var biomeCounts = new Dictionary<BiomeType, int>();
        var elementCounts = new Dictionary<ElementType, int>();

        foreach (var playerItem in teamFormation.Players)
        {
            if (playerItem == null)
            {
                continue;
            }

            var playerTable = Player.GetPlayerByItemKey(playerItem.ItemKey);
            if (playerTable == null)
            {
                continue;
            }

            biomeCounts.TryGetValue(playerTable.biomeType, out var biomeCount);
            biomeCounts[playerTable.biomeType] = biomeCount + 1;

            elementCounts.TryGetValue(playerTable.elementType, out var elementCount);
            elementCounts[playerTable.elementType] = elementCount + 1;
        }

        foreach (var synergeBiome in SynergeBiome.Table.Values)
        {
            if (!biomeCounts.TryGetValue(synergeBiome.biomeType, out var currentCount))
            {
                continue;
            }

            if (currentCount < synergeBiome.needCount)
            {
                continue;
            }

            synerges.Add(synergeBiome);
        }

        foreach (var synergeElement in SynergeElement.Table.Values)
        {
            if (!elementCounts.TryGetValue(synergeElement.elementType, out var currentCount))
            {
                continue;
            }

            if (currentCount < synergeElement.needCount)
            {
                continue;
            }

            synerges.Add(synergeElement);
        }

        return synerges;
    }
}
