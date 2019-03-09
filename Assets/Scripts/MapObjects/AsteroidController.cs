using System.Collections.Generic;
using UnityEngine;
using Imperium.Persistence;
using Imperium.Economy;
using Imperium.Persistence.MapObjects;

public class AsteroidController : MonoBehaviour, ISerializable<AsteroidControllerPersistance>
{

    public delegate void onDestroyDelegate(ResourceType resourceType, GameObject gameObject);

    public event onDestroyDelegate destroyObservers;

    public static readonly Dictionary<ResourceType, Color> asteroidColors = new Dictionary<ResourceType, Color>()
    {
          {ResourceType.Metal, Color.black},
          {ResourceType.Crystal, Color.magenta},
          {ResourceType.Energy, Color.yellow},
    };

    public ResourceType resourceType;

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
        destroyObservers(this.resourceType, this.gameObject);    
    }

    public AsteroidControllerPersistance Serialize()
    {
        return new AsteroidControllerPersistance(GetComponent<MapObject>().Serialize(), resourceQuantity, resourceType);
    }

    public void SetObject(AsteroidControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }
}