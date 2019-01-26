using Imperium.Economy;
using UnityEngine;

[RequireComponent(typeof(ObjectController))]
public class ResourceControllerStorage : MonoBehaviour
{
    public ResourceStorage resourceStorage;

    [SerializeField]
    private uint resourceCapacity;

    private void Start()
    {
        resourceStorage = new ResourceStorage(resourceCapacity);
    }
}