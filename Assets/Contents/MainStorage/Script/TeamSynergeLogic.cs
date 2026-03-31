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
        CollectCounts(teamFormation, biomeCounts, elementCounts);
        AddActiveBiomeSynerges(synerges, biomeCounts);
        AddActiveElementSynerges(synerges, elementCounts);

        return synerges;
    }

    public static List<Skill> GetAllSynergeSkills(TeamFormation teamFormation)
    {
        var skills = new List<Skill>();

        foreach (var synerge in GetAllSynerges(teamFormation))
        {
            if (string.IsNullOrWhiteSpace(synerge.synergeSkillKey))
            {
                continue;
            }

            var skillData = Skill.Get(synerge.synergeSkillKey);
            if (skillData == null)
            {
                continue;
            }

            skills.Add(skillData);
        }

        return skills;
    }

    static void CollectCounts(
        TeamFormation teamFormation,
        Dictionary<BiomeType, int> biomeCounts,
        Dictionary<ElementType, int> elementCounts)
    {
        foreach (var playerItem in teamFormation.Players)
        {
            if (playerItem == null)
            {
                continue;
            }

            var playerTable = Tables.Player.GetPlayerByItemKey(playerItem.ItemKey);
            if (playerTable == null)
            {
                continue;
            }

            biomeCounts.TryGetValue(playerTable.biomeType, out var biomeCount);
            biomeCounts[playerTable.biomeType] = biomeCount + 1;

            elementCounts.TryGetValue(playerTable.elementType, out var elementCount);
            elementCounts[playerTable.elementType] = elementCount + 1;
        }
    }

    static void AddActiveBiomeSynerges(List<ISynerge> synerges, Dictionary<BiomeType, int> biomeCounts)
    {
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
    }

    static void AddActiveElementSynerges(List<ISynerge> synerges, Dictionary<ElementType, int> elementCounts)
    {
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
    }
}
