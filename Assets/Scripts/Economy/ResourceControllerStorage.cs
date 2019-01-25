using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Economy;
[RequireComponent(typeof(ObjectController))]
public class ResourceControllerStorage : MonoBehaviour {

    public ResourceStorage resourceStorage;

    [SerializeField]
    private uint resourceCapacity;

    private void Start()
    {
        resourceStorage = new ResourceStorage(resourceCapacity);
    }

}
