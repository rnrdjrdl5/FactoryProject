using UnityEngine;

public class SpawnEntityAbility : Ability
{
    [SerializeField] EntityOwnerType ownerType;
    [SerializeField] private string defaultPrefabPath;

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

        var entity = targetEntity.AddEntity<Entity>(GetPrefabPath());
        entity.transform.position = position;
        
        return entity;
    }
}
