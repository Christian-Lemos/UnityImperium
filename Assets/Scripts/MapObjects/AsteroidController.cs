using Imperium.Economy;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour, ISerializable<AsteroidControllerPersistance>
{
    public delegate void onDestroyDelegate(ResourceType resourceType, GameObject gameObject);

    public event onDestroyDelegate destroyObservers;

    public static readonly Dictionary<ResourceType, Color> asteroidColors = new Dictionary<ResourceType, Color>()
    {
          {ResourceType.Metal, Color.white},
          {ResourceType.Crystal, Color.blue},
          {ResourceType.Energy, Color.yellow},
    };

    public ResourceType resourceType;

    public int prefabIndex;

    [SerializeField]
    private int resourceQuantity;

    public int ResourceQuantity
    {
        set
        {
            if (value <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                resourceQuantity = value;
            }
        }
        get
        {
            return resourceQuantity;
        }
    }

    private void Start()
    {
        Material material = GetComponentInChildren<Renderer>().material;
        material.color = asteroidColors[resourceType];
    }

    private void OnDestroy()
    {
        if(destroyObservers != null)
        {
             destroyObservers(resourceType, gameObject);
        }
       
    }

    public AsteroidControllerPersistance Serialize()
    {
        return new AsteroidControllerPersistance(GetComponent<MapObject>().Serialize(), resourceQuantity, resourceType, prefabIndex);
    }

    public ISerializable<AsteroidControllerPersistance> SetObject(AsteroidControllerPersistance serializedObject)
    {
        this.resourceQuantity = serializedObject.resourceQuantity;
        this.resourceType = serializedObject.resourceType;
        this.prefabIndex = serializedObject.prefabIndex;
        return this;
    }
}