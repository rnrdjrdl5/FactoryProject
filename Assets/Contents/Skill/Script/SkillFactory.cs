using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class SkillFactory
{
    static readonly Dictionary<string, Func<ProcessorAbility, SkillProcessor>> SkillProcessorFactories = new();
    static bool isInitialized;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeInitialize()
    {
        InitializeFactories();
    }

    public static void RegisterSkillProcessor(string key, Func<ProcessorAbility, SkillProcessor> factory, bool overwrite = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            Debug.LogWarning("[SkillFactory] RegisterSkillProcessor failed: key is null or empty.");
            return;
        }

        if (factory == null)
        {
            Debug.LogWarning($"[SkillFactory] RegisterSkillProcessor failed: factory is null. key={key}");
            return;
        }

        if (SkillProcessorFactories.ContainsKey(key))
        {
            if (!overwrite)
            {
                Debug.LogWarning($"[SkillFactory] RegisterSkillProcessor skipped: key already exists. key={key}");
                return;
            }
        }

        SkillProcessorFactories[key] = factory;
    }

    public static bool TryCreateSkillProcessor(ProcessorAbility ability, string key, out SkillProcessor processor)
    {
        EnsureInitialized();
        processor = null;

        if (!SkillProcessorFactories.TryGetValue(key, out var factory))
        {
            Debug.LogWarning($"[SkillFactory] SkillProcessor key not found. key={key}");
            return false;
        }

        processor = factory(ability);
        return processor != null;
    }

    public static SkillProcessor CreateSkillProcessor(ProcessorAbility ability, string key)
    {
        if (TryCreateSkillProcessor(ability, key, out var processor))
        {
            return processor;
        }

        return null;
    }

    static void EnsureInitialized()
    {
        if (isInitialized)
        {
            return;
        }

        InitializeFactories();
    }

    static void InitializeFactories()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;

        // NOTE: 외부 데이터에서 제공하는 key와 Attribute key를 매핑해 등록한다.
        foreach (var processorType in FindSkillProcessorTypes())
        {
            var attribute = processorType.GetCustomAttributes(typeof(SkillProcessorKeyAttribute), false)
                .FirstOrDefault() as SkillProcessorKeyAttribute;
            if (attribute == null)
            {
                continue;
            }

            RegisterSkillProcessor(attribute.Key, ability => CreateAndAddProcessor(ability, processorType));
        }
    }

    // NOTE : 최적화 필요, Editor에서 미리 정보를 캐싱해서 사용하던지 해야한다.
    static IEnumerable<Type> FindSkillProcessorTypes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(type => type != null).ToArray();
            }

            foreach (var type in types)
            {
                if (type == null)
                {
                    continue;
                }

                if (type.IsAbstract || !typeof(SkillProcessor).IsAssignableFrom(type))
                {
                    continue;
                }

                yield return type;
            }
        }
    }

    static SkillProcessor CreateAndAddProcessor(ProcessorAbility ability, Type processorType)
    {
        if (ability == null)
        {
            Debug.LogWarning("[SkillFactory] CreateSkillProcessor failed: ProcessorAbility is null.");
            return null;
        }

        if (Processor.Create(processorType, ability.Entity, ability) is not SkillProcessor processor)
        {
            Debug.LogWarning($"[SkillFactory] CreateSkillProcessor failed: type is not SkillProcessor. type={processorType}");
            return null;
        }

        if (processor is UpdateProcessor updateProcessor)
        {
            ability.AddDynamicProcessor(updateProcessor);
        }
        else
        {
            ability.AddDynamicProcessor(processor);
        }

        return processor;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SkillProcessorKeyAttribute : Attribute
{
    public string Key { get; }

    public SkillProcessorKeyAttribute(string key)
    {
        Key = key;
    }
}
