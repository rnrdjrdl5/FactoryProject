using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestNode
{
    public static QuestNode Create(IQuest quest)
    {
        var questNode = new QuestNode();
        questNode.targetQuest = quest;

        return questNode;
    }
    
    public IQuest TargetQuest => targetQuest;
    
    IQuest targetQuest;

    public bool ContainQuest(IQuest quest)
    {
        return ContainQuest(quest.QuestKey);
    }
    
    public bool ContainQuest(string questKey)
    {
        return questKey == targetQuest.QuestKey;
    }
}
