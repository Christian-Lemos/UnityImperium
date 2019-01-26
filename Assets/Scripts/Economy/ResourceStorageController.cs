using Imperium.Economy;
using UnityEngine;


public class ResourceStorageController : MonoBehaviour
{
    public ResourceStorage resourceStorage;

    [SerializeField]
    private uint resourceCapacity;

    private void Start()
    {
        resourceStorage = new ResourceStorage(resourceCapacity);
    }
}