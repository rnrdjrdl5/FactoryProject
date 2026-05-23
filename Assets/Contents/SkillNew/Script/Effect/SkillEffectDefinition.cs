using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffectDefinition", menuName = "SkillNew/Skill Effect Definition")]
public class SkillEffectDefinition : ScriptableObject
{
    [SerializeField] GameObject prefab;

    public GameObject Prefab => prefab;
}
