using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[DisallowMultipleComponent]
public class PixemRuntimeCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool autoBindOnAwake = true;
    [SerializeField] private bool synchronizeSpritesInLateUpdate = true;
    [SerializeField] private List<PixemPartBinding> partBindings = new List<PixemPartBinding>();
    [SerializeField] private List<PixemPartOptionIndex> partOptionIndexes = new List<PixemPartOptionIndex>();
    [SerializeField] private PixemRuntimeLoadout initialLoadout = new PixemRuntimeLoadout();

    private readonly PixemRuntimePartCatalog _catalog = new PixemRuntimePartCatalog();
    private readonly Dictionary<PixemPartType, List<PixemPartBinding>> _bindingsByType = new Dictionary<PixemPartType, List<PixemPartBinding>>();
    private readonly Dictionary<PixemPartType, PixemRuntimePartOption> _equippedOptions = new Dictionary<PixemPartType, PixemRuntimePartOption>();

    public Animator Animator => animator;

    public IReadOnlyList<PixemPartBinding> PartBindings => partBindings;
    public IReadOnlyList<PixemPartOptionIndex> PartOptionIndexes => partOptionIndexes;

    public void Configure(Animator targetAnimator, bool shouldAutoBind = true, bool shouldSyncInLateUpdate = true)
    {
        animator = targetAnimator;
        autoBindOnAwake = shouldAutoBind;
        synchronizeSpritesInLateUpdate = shouldSyncInLateUpdate;

        if (autoBindOnAwake)
        {
            AutoBindDefaultHierarchy();
        }

        RebuildBindingLookup();
        CaptureInitialLoadoutFromBindings();
        ApplyLoadout(initialLoadout);
    }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (autoBindOnAwake)
        {
            AutoBindDefaultHierarchy();
        }

        RebuildBindingLookup();
        CaptureInitialLoadoutFromBindings();
        ApplyLoadout(initialLoadout);
        SyncAllSprites();
    }

    private void LateUpdate()
    {
        if (!synchronizeSpritesInLateUpdate)
        {
            return;
        }

        SyncAllSprites();
    }

    [ContextMenu("Auto Bind Default Hierarchy")]
    public void AutoBindDefaultHierarchy()
    {
        partBindings = BuildDefaultBindings();

        for (int i = 0; i < partBindings.Count; i++)
        {
            partBindings[i].Renderer = FindRenderer(partBindings[i].HierarchyPath);
        }
    }

    [ContextMenu("Capture Initial Loadout")]
    public void CaptureInitialLoadout()
    {
        if (partBindings == null || partBindings.Count == 0)
        {
            AutoBindDefaultHierarchy();
        }

        RebuildBindingLookup();
        CaptureInitialLoadoutFromBindings();
    }

    public IReadOnlyList<PixemRuntimePartOption> GetAvailableOptions(PixemPartType partType)
    {
        return _catalog.GetOptions(partType, GetOptionEntries(partType));
    }

    public Sprite GetPreviewIcon(PixemPartType partType, string addressKey)
    {
        PixemRuntimePartOption option = _catalog.LoadOption(partType, GetOptionEntries(partType), addressKey);
        return option != null ? option.Icon : null;
    }

    public async UniTask<Sprite> GetPreviewIconAsync(PixemPartType partType, string addressKey)
    {
        PixemRuntimePartOption option = await _catalog.LoadOptionAsync(partType, GetOptionEntries(partType), addressKey);
        return option != null ? option.Icon : null;
    }

    public bool TryGetEquippedAddressKey(PixemPartType partType, out string addressKey)
    {
        if (_equippedOptions.TryGetValue(partType, out PixemRuntimePartOption option))
        {
            addressKey = option.Address;
            return true;
        }

        addressKey = string.Empty;
        return false;
    }

    public bool EquipPart(PixemPartType partType, string addressKey)
    {
        PixemRuntimePartOption option = _catalog.LoadOption(partType, GetOptionEntries(partType), addressKey);
        if (option == null)
        {
            Debug.LogWarning($"Cannot equip missing Pixem option. Part={partType}, AddressKey={addressKey}", this);
            return false;
        }

        EquipOption(partType, option);
        SyncSprites(partType);
        return true;
    }

    public async UniTask<bool> EquipPartAsync(PixemPartType partType, string addressKey)
    {
        PixemRuntimePartOption option = await _catalog.LoadOptionAsync(partType, GetOptionEntries(partType), addressKey);
        if (option == null)
        {
            Debug.LogWarning($"Cannot equip missing Pixem option. Part={partType}, AddressKey={addressKey}", this);
            return false;
        }

        EquipOption(partType, option);
        SyncSprites(partType);
        return true;
    }

    public void ResetPart(PixemPartType partType)
    {
        ResetToInitial(partType);
        SyncSprites(partType);
    }

    public void ResetAllParts()
    {
        PixemPartType[] partTypes = (PixemPartType[])Enum.GetValues(typeof(PixemPartType));
        for (int i = 0; i < partTypes.Length; i++)
        {
            ResetToInitial(partTypes[i]);
        }

        SyncAllSprites();
    }

    public void ApplyLoadout(PixemRuntimeLoadout loadout)
    {
        if (loadout == null)
        {
            return;
        }

        for (int i = 0; i < loadout.Entries.Count; i++)
        {
            PixemRuntimeLoadoutEntry entry = loadout.Entries[i];
            if (string.IsNullOrEmpty(entry.AddressKey))
            {
                continue;
            }

            PixemRuntimePartOption option = _catalog.LoadOption(entry.PartType, GetOptionEntries(entry.PartType), entry.AddressKey);
            if (option != null)
            {
                EquipOption(entry.PartType, option);
            }
        }

        SyncAllSprites();
    }

    public PixemRuntimeLoadout CaptureCurrentLoadout()
    {
        PixemRuntimeLoadout loadout = new PixemRuntimeLoadout();

        foreach (KeyValuePair<PixemPartType, PixemRuntimePartOption> pair in _equippedOptions)
        {
            loadout.SetAddressKey(pair.Key, pair.Value.Address);
        }

        return loadout;
    }

    public void SyncAllSprites()
    {
        foreach (KeyValuePair<PixemPartType, List<PixemPartBinding>> pair in _bindingsByType)
        {
            SyncSprites(pair.Key);
        }
    }

    private void SyncSprites(PixemPartType partType)
    {
        if (!_bindingsByType.TryGetValue(partType, out List<PixemPartBinding> bindings))
        {
            return;
        }

        if (!_equippedOptions.TryGetValue(partType, out PixemRuntimePartOption option))
        {
            return;
        }

        for (int i = 0; i < bindings.Count; i++)
        {
            PixemPartBinding binding = bindings[i];
            if (binding.Renderer == null)
            {
                continue;
            }

            Sprite currentSprite = binding.Renderer.sprite;
            int frameIndex = ExtractFrameIndex(currentSprite != null ? currentSprite.name : string.Empty);
            Sprite targetSprite = option.GetSpriteByFrameIndex(frameIndex);

            if (targetSprite != null && binding.Renderer.sprite != targetSprite)
            {
                binding.Renderer.sprite = targetSprite;
            }
        }
    }

    private void EquipOption(PixemPartType partType, PixemRuntimePartOption option)
    {
        _equippedOptions[partType] = option;
        ApplyExclusiveRules(partType, option);
    }

    private void ApplyExclusiveRules(PixemPartType partType, PixemRuntimePartOption option)
    {
        if (!option.IsEmpty)
        {
            switch (partType)
            {
                case PixemPartType.Face:
                case PixemPartType.FaceAcc1:
                case PixemPartType.FaceAcc2:
                case PixemPartType.Hair:
                case PixemPartType.HairAcc:
                    ResetToInitial(PixemPartType.Helmet);
                    break;
                case PixemPartType.Helmet:
                    ResetToInitial(PixemPartType.Face);
                    ResetToInitial(PixemPartType.FaceAcc1);
                    ResetToInitial(PixemPartType.FaceAcc2);
                    ResetToInitial(PixemPartType.Hair);
                    ResetToInitial(PixemPartType.HairAcc);
                    break;
                case PixemPartType.RightHandWeapon:
                    ResetToInitial(PixemPartType.Shield);
                    break;
                case PixemPartType.Shield:
                    ResetToInitial(PixemPartType.RightHandWeapon);
                    break;
            }
        }
    }

    private void ResetToInitial(PixemPartType partType)
    {
        string addressKey = initialLoadout.GetAddressKey(partType);
        if (string.IsNullOrEmpty(addressKey))
        {
            return;
        }

        PixemRuntimePartOption option = _catalog.LoadOption(partType, GetOptionEntries(partType), addressKey);
        if (option != null)
        {
            _equippedOptions[partType] = option;
        }
    }

    private void CaptureInitialLoadoutFromBindings()
    {
        if (initialLoadout == null)
        {
            initialLoadout = new PixemRuntimeLoadout();
        }

        PixemPartType[] partTypes = (PixemPartType[])Enum.GetValues(typeof(PixemPartType));
        for (int i = 0; i < partTypes.Length; i++)
        {
            PixemPartType partType = partTypes[i];
            string detectedAddressKey = initialLoadout.GetAddressKey(partType);

            if (string.IsNullOrEmpty(detectedAddressKey))
            {
                List<PixemPartBinding> bindings = GetBindings(partType);
                detectedAddressKey = DetectAddressKeyFromBindings(partType, bindings);
            }

            if (string.IsNullOrEmpty(detectedAddressKey))
            {
                IReadOnlyList<PixemRuntimePartOption> options = _catalog.GetOptions(partType, GetOptionEntries(partType));
                if (options.Count == 0)
                {
                    continue;
                }

                detectedAddressKey = options[0].Address;
            }

            initialLoadout.SetAddressKey(partType, detectedAddressKey);

            PixemRuntimePartOption option = _catalog.LoadOption(partType, GetOptionEntries(partType), detectedAddressKey);
            if (option != null)
            {
                _equippedOptions[partType] = option;
            }
        }
    }

    private string DetectAddressKeyFromBindings(PixemPartType partType, List<PixemPartBinding> bindings)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            PixemPartBinding binding = bindings[i];
            if (binding == null || binding.Renderer == null || binding.Renderer.sprite == null)
            {
                continue;
            }

            Texture texture = binding.Renderer.sprite.texture;
            if (texture != null)
            {
                IReadOnlyList<PixemPartOptionEntry> optionEntries = GetOptionEntries(partType);
                for (int entryIndex = 0; entryIndex < optionEntries.Count; entryIndex++)
                {
                    if (optionEntries[entryIndex].OptionId == texture.name)
                    {
                        return optionEntries[entryIndex].AddressKey;
                    }
                }
            }
        }

        return string.Empty;
    }

    private List<PixemPartBinding> GetBindings(PixemPartType partType)
    {
        if (_bindingsByType.TryGetValue(partType, out List<PixemPartBinding> bindings))
        {
            return bindings;
        }

        return new List<PixemPartBinding>();
    }

    private IReadOnlyList<PixemPartOptionEntry> GetOptionEntries(PixemPartType partType)
    {
        for (int i = 0; i < partOptionIndexes.Count; i++)
        {
            if (partOptionIndexes[i].PartType == partType)
            {
                return partOptionIndexes[i].Options;
            }
        }

        return Array.Empty<PixemPartOptionEntry>();
    }

#if UNITY_EDITOR
    [ContextMenu("Rebuild Part Option Index")]
    public void RebuildPartOptionIndex()
    {
        string rootPath = System.IO.Path.Combine(Application.dataPath, "Contents/Pixem/BuiltIn/Parts");
        PixemPartType[] partTypes = (PixemPartType[])Enum.GetValues(typeof(PixemPartType));
        List<PixemPartOptionIndex> rebuiltIndexes = new List<PixemPartOptionIndex>(partTypes.Length);

        for (int i = 0; i < partTypes.Length; i++)
        {
            string folderName = GetFolderName(partTypes[i]);
            string folderPath = System.IO.Path.Combine(rootPath, folderName);
            List<PixemPartOptionEntry> options = new List<PixemPartOptionEntry>();

            if (System.IO.Directory.Exists(folderPath))
            {
                string[] files = System.IO.Directory.GetFiles(folderPath, "*.png");
                for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
                {
                    string optionId = System.IO.Path.GetFileNameWithoutExtension(files[fileIndex]);
                    options.Add(new PixemPartOptionEntry
                    {
                        OptionId = optionId,
                        AddressKey = "Pixem/Parts/" + folderName + "/" + optionId
                    });
                }

                options.Sort(CompareOptionEntries);
            }

            rebuiltIndexes.Add(new PixemPartOptionIndex
            {
                PartType = partTypes[i],
                Options = options
            });
        }

        partOptionIndexes = rebuiltIndexes;
    }
#endif

    private void RebuildBindingLookup()
    {
        _bindingsByType.Clear();

        for (int i = 0; i < partBindings.Count; i++)
        {
            PixemPartBinding binding = partBindings[i];
            if (binding == null)
            {
                continue;
            }

            if (!_bindingsByType.TryGetValue(binding.PartType, out List<PixemPartBinding> bindings))
            {
                bindings = new List<PixemPartBinding>();
                _bindingsByType[binding.PartType] = bindings;
            }

            bindings.Add(binding);
        }
    }

    private SpriteRenderer FindRenderer(string hierarchyPath)
    {
        Transform target = transform.Find(hierarchyPath);
        return target != null ? target.GetComponent<SpriteRenderer>() : null;
    }

    private static List<PixemPartBinding> BuildDefaultBindings()
    {
        return new List<PixemPartBinding>
        {
            new PixemPartBinding { PartType = PixemPartType.Cape, HierarchyPath = "Model/Cape" },
            new PixemPartBinding { PartType = PixemPartType.Body, HierarchyPath = "Model/Body" },
            new PixemPartBinding { PartType = PixemPartType.Body, HierarchyPath = "Model/RightHand" },
            new PixemPartBinding { PartType = PixemPartType.Body, HierarchyPath = "Model/LeftHand" },
            new PixemPartBinding { PartType = PixemPartType.Body, HierarchyPath = "Model/LeftHand/LeftArm" },
            new PixemPartBinding { PartType = PixemPartType.Pants, HierarchyPath = "Model/Pants" },
            new PixemPartBinding { PartType = PixemPartType.Top, HierarchyPath = "Model/Top" },
            new PixemPartBinding { PartType = PixemPartType.Face, HierarchyPath = "Model/Head/Face" },
            new PixemPartBinding { PartType = PixemPartType.FaceAcc1, HierarchyPath = "Model/Head/FaceAcc_1" },
            new PixemPartBinding { PartType = PixemPartType.FaceAcc2, HierarchyPath = "Model/Head/FaceAcc_2" },
            new PixemPartBinding { PartType = PixemPartType.Hair, HierarchyPath = "Model/Head/Hair" },
            new PixemPartBinding { PartType = PixemPartType.HairAcc, HierarchyPath = "Model/Head/HairAcc" },
            new PixemPartBinding { PartType = PixemPartType.Helmet, HierarchyPath = "Model/Head/Helmet" },
            new PixemPartBinding { PartType = PixemPartType.RightHandWeapon, HierarchyPath = "Model/RightHand/RightHandWeapon" },
            new PixemPartBinding { PartType = PixemPartType.Shield, HierarchyPath = "Model/RightHand/Shield" },
            new PixemPartBinding { PartType = PixemPartType.LeftHandWeapon, HierarchyPath = "Model/LeftHand/LeftHandWeapon" }
        };
    }

    private static int ExtractFrameIndex(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return -1;
        }

        int separatorIndex = value.LastIndexOf('_');
        if (separatorIndex < 0 || separatorIndex >= value.Length - 1)
        {
            return -1;
        }

        return int.TryParse(value.Substring(separatorIndex + 1), out int frameIndex)
            ? frameIndex
            : -1;
    }

    private static string GetFolderName(PixemPartType partType)
    {
        switch (partType)
        {
            case PixemPartType.FaceAcc1:
                return "FaceAcc_1";
            case PixemPartType.FaceAcc2:
                return "FaceAcc_2";
            default:
                return partType.ToString();
        }
    }

    private static int CompareOptionEntries(PixemPartOptionEntry left, PixemPartOptionEntry right)
    {
        bool leftEmpty = left.OptionId.StartsWith("Empty_", StringComparison.Ordinal);
        bool rightEmpty = right.OptionId.StartsWith("Empty_", StringComparison.Ordinal);

        if (leftEmpty && !rightEmpty)
        {
            return -1;
        }

        if (!leftEmpty && rightEmpty)
        {
            return 1;
        }

        int leftIndex = ExtractFrameIndex(left.OptionId);
        int rightIndex = ExtractFrameIndex(right.OptionId);

        if (leftIndex != rightIndex)
        {
            return leftIndex.CompareTo(rightIndex);
        }

        return string.Compare(left.OptionId, right.OptionId, StringComparison.Ordinal);
    }
}

[Serializable]
public class PixemPartBinding
{
    public PixemPartType PartType;
    public string HierarchyPath;
    public SpriteRenderer Renderer;
}

[Serializable]
public class PixemPartOptionIndex
{
    public PixemPartType PartType;
    public List<PixemPartOptionEntry> Options = new List<PixemPartOptionEntry>();
}

[Serializable]
public class PixemPartOptionEntry
{
    public string OptionId;
    public string AddressKey;
}
