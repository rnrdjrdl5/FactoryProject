using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PixemAnimationFrameCache", menuName = "Pixem/Animation Frame Cache")]
public sealed class PixemAnimationFrameCache : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController sourceController;
    [SerializeField] private List<PixemAnimationBindingFrameCache> bindings = new List<PixemAnimationBindingFrameCache>();

    private Dictionary<PixemPartType, Dictionary<string, Dictionary<Sprite, int>>> _lookupByPartAndPath;

    public RuntimeAnimatorController SourceController => sourceController;
    public IReadOnlyList<PixemAnimationBindingFrameCache> Bindings => bindings;

    public bool HasCachedFrames => bindings != null && bindings.Count > 0;

    public bool TryGetFrameIndex(PixemPartType partType, string hierarchyPath, Sprite sprite, out int frameIndex)
    {
        frameIndex = -1;
        if (sprite == null)
        {
            return false;
        }

        EnsureLookupBuilt();
        if (_lookupByPartAndPath == null)
        {
            return false;
        }

        if (!_lookupByPartAndPath.TryGetValue(partType, out Dictionary<string, Dictionary<Sprite, int>> lookupByPath))
        {
            return false;
        }

        string cachePath = hierarchyPath ?? string.Empty;
        if (!lookupByPath.TryGetValue(cachePath, out Dictionary<Sprite, int> lookupBySprite))
        {
            return false;
        }

        return lookupBySprite.TryGetValue(sprite, out frameIndex);
    }

    private void EnsureLookupBuilt()
    {
        if (_lookupByPartAndPath != null)
        {
            return;
        }

        _lookupByPartAndPath = new Dictionary<PixemPartType, Dictionary<string, Dictionary<Sprite, int>>>();
        if (bindings == null)
        {
            return;
        }

        for (int i = 0; i < bindings.Count; i++)
        {
            PixemAnimationBindingFrameCache binding = bindings[i];
            if (binding == null)
            {
                continue;
            }

            if (!_lookupByPartAndPath.TryGetValue(binding.PartType, out Dictionary<string, Dictionary<Sprite, int>> lookupByPath))
            {
                lookupByPath = new Dictionary<string, Dictionary<Sprite, int>>(StringComparer.Ordinal);
                _lookupByPartAndPath[binding.PartType] = lookupByPath;
            }

            string cachePath = binding.HierarchyPath ?? string.Empty;
            if (!lookupByPath.TryGetValue(cachePath, out Dictionary<Sprite, int> lookupBySprite))
            {
                lookupBySprite = new Dictionary<Sprite, int>();
                lookupByPath[cachePath] = lookupBySprite;
            }

            List<PixemAnimationFrameEntry> frameEntries = binding.FrameEntries;
            for (int entryIndex = 0; entryIndex < frameEntries.Count; entryIndex++)
            {
                PixemAnimationFrameEntry entry = frameEntries[entryIndex];
                if (entry.Sprite == null || lookupBySprite.ContainsKey(entry.Sprite))
                {
                    continue;
                }

                lookupBySprite.Add(entry.Sprite, entry.FrameIndex);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Rebuild From Source Controller")]
    public void RebuildFromSourceController()
    {
        RebuildFromBindings(PixemRuntimeCharacter.BuildDefaultBindingsSnapshot());
    }

    public void RebuildFromBindings(IReadOnlyList<PixemPartBinding> partBindings)
    {
        bindings = PixemAnimationFrameCacheBuilder.Build(sourceController, partBindings);
        _lookupByPartAndPath = null;
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}

[Serializable]
public sealed class PixemAnimationBindingFrameCache
{
    [SerializeField] private PixemPartType partType;
    [SerializeField] private string hierarchyPath;
    [SerializeField] private List<PixemAnimationFrameEntry> frameEntries = new List<PixemAnimationFrameEntry>();

    public PixemPartType PartType
    {
        get => partType;
        set => partType = value;
    }

    public string HierarchyPath
    {
        get => hierarchyPath;
        set => hierarchyPath = value;
    }

    public List<PixemAnimationFrameEntry> FrameEntries
    {
        get => frameEntries;
        set => frameEntries = value ?? new List<PixemAnimationFrameEntry>();
    }
}

[Serializable]
public sealed class PixemAnimationFrameEntry
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int frameIndex;

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public int FrameIndex
    {
        get => frameIndex;
        set => frameIndex = value;
    }
}

#if UNITY_EDITOR
internal static class PixemAnimationFrameCacheBuilder
{
    public static List<PixemAnimationBindingFrameCache> Build(RuntimeAnimatorController controller, IReadOnlyList<PixemPartBinding> partBindings)
    {
        var rebuiltBindings = new List<PixemAnimationBindingFrameCache>();
        if (controller == null || partBindings == null || partBindings.Count == 0)
        {
            return rebuiltBindings;
        }

        var spriteBindingsByPath = new Dictionary<string, PixemAnimationBindingFrameCache>(StringComparer.Ordinal);
        for (int i = 0; i < partBindings.Count; i++)
        {
            PixemPartBinding partBinding = partBindings[i];
            if (partBinding == null || string.IsNullOrEmpty(partBinding.HierarchyPath))
            {
                continue;
            }

            if (spriteBindingsByPath.ContainsKey(partBinding.HierarchyPath))
            {
                continue;
            }

            PixemAnimationBindingFrameCache cache = new PixemAnimationBindingFrameCache
            {
                PartType = partBinding.PartType,
                HierarchyPath = partBinding.HierarchyPath,
                FrameEntries = new List<PixemAnimationFrameEntry>()
            };
            spriteBindingsByPath.Add(cache.HierarchyPath, cache);
            rebuiltBindings.Add(cache);
        }

        AnimationClip[] clips = controller.animationClips;
        for (int clipIndex = 0; clipIndex < clips.Length; clipIndex++)
        {
            AnimationClip clip = clips[clipIndex];
            if (clip == null)
            {
                continue;
            }

            EditorExtractClipFrames(clip, spriteBindingsByPath);
        }

        for (int i = 0; i < rebuiltBindings.Count; i++)
        {
            rebuiltBindings[i].FrameEntries.Sort(CompareEntries);
        }

        return rebuiltBindings;
    }

    private static void EditorExtractClipFrames(AnimationClip clip, Dictionary<string, PixemAnimationBindingFrameCache> spriteBindingsByPath)
    {
        UnityEditor.EditorCurveBinding[] curveBindings = UnityEditor.AnimationUtility.GetObjectReferenceCurveBindings(clip);
        for (int bindingIndex = 0; bindingIndex < curveBindings.Length; bindingIndex++)
        {
            UnityEditor.EditorCurveBinding curveBinding = curveBindings[bindingIndex];
            if (!IsSpriteRendererBinding(curveBinding))
            {
                continue;
            }

            if (!spriteBindingsByPath.TryGetValue(curveBinding.path, out PixemAnimationBindingFrameCache cache))
            {
                continue;
            }

            UnityEditor.ObjectReferenceKeyframe[] keyframes = UnityEditor.AnimationUtility.GetObjectReferenceCurve(clip, curveBinding);
            for (int keyframeIndex = 0; keyframeIndex < keyframes.Length; keyframeIndex++)
            {
                Sprite sprite = keyframes[keyframeIndex].value as Sprite;
                if (sprite == null || ContainsSprite(cache.FrameEntries, sprite))
                {
                    continue;
                }

                cache.FrameEntries.Add(new PixemAnimationFrameEntry
                {
                    Sprite = sprite,
                    FrameIndex = ResolveFrameIndex(sprite, cache.FrameEntries.Count)
                });
            }
        }
    }

    private static bool ContainsSprite(List<PixemAnimationFrameEntry> entries, Sprite sprite)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].Sprite == sprite)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSpriteRendererBinding(UnityEditor.EditorCurveBinding curveBinding)
    {
        return curveBinding.type == typeof(SpriteRenderer)
            && string.Equals(curveBinding.propertyName, "m_Sprite", StringComparison.Ordinal);
    }

    private static int ResolveFrameIndex(Sprite sprite, int fallbackIndex)
    {
        int parsedIndex = ExtractFrameIndex(sprite != null ? sprite.name : string.Empty);
        return parsedIndex >= 0 ? parsedIndex : fallbackIndex;
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

    private static int CompareEntries(PixemAnimationFrameEntry left, PixemAnimationFrameEntry right)
    {
        int frameCompare = left.FrameIndex.CompareTo(right.FrameIndex);
        if (frameCompare != 0)
        {
            return frameCompare;
        }

        string leftName = left.Sprite != null ? left.Sprite.name : string.Empty;
        string rightName = right.Sprite != null ? right.Sprite.name : string.Empty;
        return string.Compare(leftName, rightName, StringComparison.Ordinal);
    }
}
#endif
