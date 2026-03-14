using UnityEngine;


public class FactoryEntry : Entry
{
    // NOTE : 직접 접근은 좋지 않다. 다른 방법 생각해보기.
    public static MainStorage MainStorage
    {
        get
        {
            if (mainStorage == null)
            {
                mainStorage = RootRealm.GetChild<MainStorage>();
            }

            return mainStorage;
        }
    }

    static MainStorage mainStorage = null;
}
