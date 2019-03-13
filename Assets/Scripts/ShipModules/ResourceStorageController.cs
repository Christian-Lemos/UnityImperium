using Imperium.Economy;
using Imperium.Persistence;
using Imperium.Persistence.ShipModules;
using UnityEngine;

public class ResourceStorageController : MonoBehaviour, ISerializable<ResourceStoragePersistance>
{
    public ResourceStorage resourceStorage;

    [SerializeField]
    private uint resourceCapacity;

    public ResourceStoragePersistance Serialize()
    {
        return new ResourceStoragePersistance(resourceStorage.maximumResourcesStorage, resourceStorage.ToResourceQuantities());
    }

    public ISerializable<ResourceStoragePersistance> SetObject(ResourceStoragePersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        resourceStorage = new ResourceStorage(resourceCapacity);
    }
}