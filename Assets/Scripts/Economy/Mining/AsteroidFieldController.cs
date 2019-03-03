using Imperium.Enum;
using System.Collections;
using System.Collections.Generic;
using Imperium.MapObjects;
using UnityEngine;
[SerializePrivateVariables]
public class AsteroidFieldController : MonoBehaviour {

    public AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings;

    

    private Dictionary<ResourceType, HashSet<GameObject>> _asteroids = new Dictionary<ResourceType, HashSet<GameObject>>();

    

    

    /**
     *  This is for calculations of new asteroids spawn position.
     *  Min and Max are the minimum and max values of it's respective axis
     *  Offset is the minimum distance bettwen each asteroid of it's respective axis
     *  The rotation must be (0, 0, 0) 
     */
    private float asteroidMinXAxis;
    private float asteroidMaxXAxis;

    private float asteroidMinYAxis;
    private float asteroidMaxYAxis;

    private float asteroidMinZAxis;
    private float asteroidMaxZAxis;

    public Vector3 size;

    private void Start()
    {
        
        
    }

    private void OnEnable()
    {
        foreach(ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            _asteroids.Add(resourceType, new HashSet<GameObject>());
        }

        asteroidMinXAxis = this.transform.position.x - size.x / 2;
        asteroidMaxXAxis = this.transform.position.x + size.x / 2;
        

        asteroidMinYAxis = this.transform.position.y - size.y / 2;
        asteroidMaxYAxis = this.transform.position.y + size.y / 2;
        

        asteroidMinZAxis = this.transform.position.z - size.z / 2;
        asteroidMaxZAxis = this.transform.position.z + size.z / 2;
 

       /* AsteroidController[] asteroidControllers = GetComponentsInChildren<AsteroidController>();

        foreach(AsteroidController asteroidController in asteroidControllers)
        {
            ResourceType resourceType = asteroidController.resourceType;

            _asteroids[resourceType].Add(asteroidController.gameObject);
           
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, this.size);
    }


    public Dictionary<ResourceType, HashSet<GameObject>> Asteroids
    {
        get
        {
            return _asteroids;
        }

        set
        {
            _asteroids = value;
        }
    }

    public void SpawnAsteroidsOnField(bool destroyCurrentAsteroids)
    {
        if(destroyCurrentAsteroids)
        {
           foreach(ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
           {
                _asteroids[resourceType].Clear();
           }

            
            AsteroidController[] asteroidControllers = GetComponentsInChildren<AsteroidController>();

            foreach(AsteroidController asteroidController in asteroidControllers)
            {
                Destroy(asteroidController.gameObject);
            }
        }

        StartCoroutine(FieldSpawnerEnumerator());

        
    }


    public GameObject SpawnAsteroid(ResourceType resourceType, Vector3 position)
    {
        HashSet<GameObject> asteroidsOfType = _asteroids[resourceType];

        if(asteroidsOfType.Count >= this.asteroidFieldAsteroidSettings.asteroidTypeQuantity[resourceType]) //Checks if there is "room" for the new asteroid
        {
            throw new System.Exception("Not enough room for the new asteroid");
        }
        else
        {
           
            GameObject asteroid = Spawner.Instance.SpawnAsteroid(this, resourceType, (int) this.asteroidFieldAsteroidSettings.resourceQuantityOfResourceType[resourceType], position, false);

            _asteroids[resourceType].Add(asteroid);

            asteroid.SetActive(true);

            return asteroid;
        }
    }

    private IEnumerator FieldSpawnerEnumerator()
    {

        foreach(KeyValuePair<ResourceType, uint> keyValuePair in asteroidFieldAsteroidSettings.asteroidTypeQuantity)
        {
            for(uint i = 0; i < keyValuePair.Value; i++)
            {
                System.Random random = new System.Random();

                float positionX = (float) random.NextDouble() * (asteroidMaxXAxis - asteroidMinXAxis) + asteroidMinXAxis;
                float positionY =  (float) random.NextDouble() * (asteroidMaxYAxis - asteroidMinYAxis) + asteroidMinYAxis;       
                float positionZ = (float) random.NextDouble() * (asteroidMaxZAxis - asteroidMinZAxis) + asteroidMinZAxis;

                Vector3 position = new Vector3(positionX, positionY, positionZ);
                SpawnAsteroid(keyValuePair.Key, position);
                yield return null;
            }
        }

        


    }

}
