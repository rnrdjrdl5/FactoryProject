using System;
using System.Collections.Generic;

[Serializable]
public class PixemRuntimeLoadout
{
    public List<PixemRuntimeLoadoutEntry> Entries = new List<PixemRuntimeLoadoutEntry>();

    public string GetAddressKey(PixemPartType partType)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (Entries[i].PartType == partType)
            {
                return Entries[i].AddressKey;
            }
        }

        return string.Empty;
    }

    public void SetAddressKey(PixemPartType partType, string addressKey)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (Entries[i].PartType == partType)
            {
                Entries[i].AddressKey = addressKey;
                return;
            }
        }

        Entries.Add(new PixemRuntimeLoadoutEntry
        {
            PartType = partType,
            AddressKey = addressKey
        });
    }
}

[Serializable]
public class PixemRuntimeLoadoutEntry
{
    public PixemPartType PartType;
    public string AddressKey;
}
