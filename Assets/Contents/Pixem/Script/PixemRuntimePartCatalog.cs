using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public sealed class PixemRuntimePartCatalog
{
    private readonly Dictionary<PixemPartType, Dictionary<string, PixemRuntimePartOption>> _optionsByAddress = new Dictionary<PixemPartType, Dictionary<string, PixemRuntimePartOption>>();

    public IReadOnlyList<PixemRuntimePartOption> GetOptions(PixemPartType partType, IReadOnlyList<PixemPartOptionEntry> optionEntries)
    {
        EnsureIndexed(partType, optionEntries);

        List<PixemRuntimePartOption> options = new List<PixemRuntimePartOption>(optionEntries.Count);
        Dictionary<string, PixemRuntimePartOption> optionsByAddress = _optionsByAddress[partType];
        for (int i = 0; i < optionEntries.Count; i++)
        {
            if (optionsByAddress.TryGetValue(optionEntries[i].AddressKey, out PixemRuntimePartOption option))
            {
                options.Add(option);
            }
        }

        return options;
    }

    public bool TryGetOption(PixemPartType partType, IReadOnlyList<PixemPartOptionEntry> optionEntries, string addressKey, out PixemRuntimePartOption option)
    {
        EnsureIndexed(partType, optionEntries);
        if (_optionsByAddress[partType].TryGetValue(addressKey, out option))
        {
            return true;
        }

        if (string.IsNullOrEmpty(addressKey))
        {
            option = null;
            return false;
        }

        option = CreateFallbackOption(partType, addressKey);
        _optionsByAddress[partType][addressKey] = option;
        return true;
    }

    public PixemRuntimePartOption LoadOption(PixemPartType partType, IReadOnlyList<PixemPartOptionEntry> optionEntries, string addressKey)
    {
        if (!TryGetOption(partType, optionEntries, addressKey, out PixemRuntimePartOption option))
        {
            return null;
        }

        EnsureOptionLoaded(option);
        return option;
    }

    public async UniTask<PixemRuntimePartOption> LoadOptionAsync(PixemPartType partType, IReadOnlyList<PixemPartOptionEntry> optionEntries, string addressKey)
    {
        if (!TryGetOption(partType, optionEntries, addressKey, out PixemRuntimePartOption option))
        {
            return null;
        }

        await EnsureOptionLoadedAsync(option);
        return option;
    }

    private void EnsureIndexed(PixemPartType partType, IReadOnlyList<PixemPartOptionEntry> optionEntries)
    {
        if (_optionsByAddress.ContainsKey(partType))
        {
            return;
        }

        Dictionary<string, PixemRuntimePartOption> optionsByAddress = new Dictionary<string, PixemRuntimePartOption>(StringComparer.Ordinal);

        for (int i = 0; i < optionEntries.Count; i++)
        {
            PixemPartOptionEntry entry = optionEntries[i];
            PixemRuntimePartOption option = new PixemRuntimePartOption(partType, entry.OptionId, entry.AddressKey);
            optionsByAddress[option.Address] = option;
        }
        _optionsByAddress[partType] = optionsByAddress;
    }

    private static PixemRuntimePartOption CreateFallbackOption(PixemPartType partType, string addressKey)
    {
        int separatorIndex = addressKey.LastIndexOf('/');
        string optionId = separatorIndex >= 0 && separatorIndex < addressKey.Length - 1
            ? addressKey.Substring(separatorIndex + 1)
            : addressKey;

        return new PixemRuntimePartOption(partType, optionId, addressKey);
    }

    private void EnsureOptionLoaded(PixemRuntimePartOption option)
    {
        if (option.IsLoaded)
        {
            return;
        }

        Texture2D texture = Realm.LoadResources<Texture2D>(option.Address);
        FinalizeLoadedOption(option, texture);
    }

    private async UniTask EnsureOptionLoadedAsync(PixemRuntimePartOption option)
    {
        if (option.IsLoaded)
        {
            return;
        }

        Texture2D texture = await Realm.LoadResourceAsync<Texture2D>(option.Address);
        FinalizeLoadedOption(option, texture);
    }

    private void FinalizeLoadedOption(PixemRuntimePartOption option, Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogWarning("Failed to load Pixem part texture: " + option.Address);
            return;
        }

        option.Texture = texture;
        option.Sprites = SliceSprites(option.PartType, option.OptionId, texture);
        option.Icon = option.Sprites.Length > 0 ? option.Sprites[0] : null;
        option.IsLoaded = true;
    }

    private static Sprite[] SliceSprites(PixemPartType partType, string optionId, Texture2D texture)
    {
        int spriteWidth = partType == PixemPartType.LeftHandWeapon ? 64 : 32;
        int spriteHeight = partType == PixemPartType.LeftHandWeapon ? 64 : 32;
        int rows = texture.height / spriteHeight;
        int columns = texture.width / spriteWidth;
        List<Sprite> sprites = new List<Sprite>(rows * columns);

        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Rect rect = new Rect(
                    x * spriteWidth,
                    texture.height - ((y + 1) * spriteHeight),
                    spriteWidth,
                    spriteHeight);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 32f);
                sprite.name = optionId + "_" + index++;
                sprites.Add(sprite);
            }
        }

        return sprites.ToArray();
    }

}

public sealed class PixemRuntimePartOption
{
    public PixemRuntimePartOption(PixemPartType partType, string optionId, string address)
    {
        PartType = partType;
        OptionId = optionId;
        Address = address;
        Texture = null;
        Sprites = Array.Empty<Sprite>();
        Icon = null;
        IsEmpty = optionId.StartsWith("Empty_", StringComparison.Ordinal);
        IsLoaded = false;
    }

    public PixemPartType PartType { get; }
    public string OptionId { get; }
    public string Address { get; }
    public Texture2D Texture { get; internal set; }
    public Sprite[] Sprites { get; internal set; }
    public Sprite Icon { get; internal set; }
    public bool IsEmpty { get; }
    public bool IsLoaded { get; internal set; }

    public Sprite GetSpriteByFrameIndex(int frameIndex)
    {
        if (Sprites == null || Sprites.Length == 0)
        {
            return null;
        }

        if (frameIndex < 0)
        {
            return Sprites[0];
        }

        if (frameIndex >= Sprites.Length)
        {
            return Sprites[Sprites.Length - 1];
        }

        return Sprites[frameIndex];
    }
}
