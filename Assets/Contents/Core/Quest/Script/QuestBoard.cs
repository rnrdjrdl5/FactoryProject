using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    List<QuestNode> quests;

    public bool TryAddQuest(IQuest addQuest)
    {
        if (quests.Any(quest => quest.ContainQuest(addQuest)))
        {
            return false;
        }

        var questNode = QuestNode.Create(addQuest);
        quests.Add(questNode);
        
        return true;
    }

    public bool TryRemoveQuest(IQuest removeQuest)
    {
        var questNode = quests.FirstOrDefault(quest => quest == removeQuest);
        if (questNode == null)
        {
            return false;
        }
        
        quests.Remove(questNode);
        return true;
    }

    public bool TryGetQuest(string questKey, out IQuest quest)
    {
        quest = null;
        var questNode = quests.FirstOrDefault(q => q.ContainQuest(questKey));
        if (questNode == null)
        {
            return false;
        }
        
        quest = questNode.TargetQuest;
        return true;
    }
}