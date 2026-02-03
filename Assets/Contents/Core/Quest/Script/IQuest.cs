using System.Collections.Generic;

public interface IQuest
{
    string QuestKey { get; }

    bool ClearQuest();
}