using UnityEngine;

public class SpawnEntityAbility : Ability
{
    [SerializeField] EntityOwnerType ownerType;
    [SerializeField] string defaultPrefabPath;

    string? prefabPath;

    public void SetPrefabPath(string path)
    {
        prefabPath = path;
    }

    string GetPrefabPath()
    {
        return prefabPath ?? defaultPrefabPath;
    }
    
    public Entity SpawnEntity(Vector3 position)
    {
        var targetEntity = Entity;
        if (ownerType == EntityOwnerType.Parent)
        {
            targetEntity = Entity.Parent;
        }

        var prefabPath = GetPrefabPath();
        if (string.IsNullOrEmpty(prefabPath))
        {
            return null;
        }

        var entity = targetEntity.AddEntity<Entity>(prefabPath);
        entity.transform.position = position;
        
        return entity;
    }
}
