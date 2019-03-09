using System.Collections.Generic;
using UnityEngine;
using Imperium.Persistence;
using Imperium.Economy;

public class AsteroidController : MonoBehaviour, ISerializable<AsteroidPersistance>
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

    public AsteroidPersistance Serialize()
    {
        return new AsteroidPersistance(transform.position, transform.rotation, transform.localScale, resourceType, resourceQuantity);
    }

    public void SetObject(AsteroidPersistance serializedObject)
    {
        this.transform.position = serializedObject.position;
        this.transform.rotation = serializedObject.rotation;
        this.transform.localScale = serializedObject.scale;

        this.resourceType = serializedObject.resourceType;
        this.resourceQuantity = serializedObject.resourceQuantity;
    }
}