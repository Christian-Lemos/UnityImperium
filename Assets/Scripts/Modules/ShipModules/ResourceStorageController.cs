using Imperium.Economy;
using Imperium.Persistence;
using Imperium.Persistence.ShipModules;
using UnityEngine;

public class ResourceStorageController : MonoBehaviour, ISerializable<ResourceStoragePersistance>
{
    [SerializeField]
    private uint resourceCapacity;

    public ResourceStorage ResourceStorage { get; set; }

    public ResourceStoragePersistance Serialize()
    {
        return new ResourceStoragePersistance(ResourceStorage.maximumResourcesStorage, ResourceStorage.ToResourceQuantities());
    }

    public ISerializable<ResourceStoragePersistance> SetObject(ResourceStoragePersistance serializedObject)
    {
        ResourceStorage = ResourceStorage.FromResourceQuantities(serializedObject.maximumStorage, serializedObject.resourceQuantities);
        resourceCapacity = serializedObject.maximumStorage;
        return this;
    }

    private void Start()
    {
        if (ResourceStorage == null)
        {
            ResourceStorage = new ResourceStorage(resourceCapacity);
        }
    }
}