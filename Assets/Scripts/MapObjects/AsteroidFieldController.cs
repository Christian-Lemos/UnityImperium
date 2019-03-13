using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldController : MonoBehaviour, ISerializable<AsteroidFieldControllerPersistance>
{
    public AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings;
    public bool initialized = false;
    public Vector3 size;
    private Dictionary<ResourceType, HashSet<GameObject>> _asteroids = new Dictionary<ResourceType, HashSet<GameObject>>();

    /**
     *  This is for calculations of new asteroids spawn position.
     *  Min and Max are the minimum and max values of it's respective axis
     *  Offset is the minimum distance bettwen each asteroid of it's respective axis
     *  The rotation must be (0, 0, 0)
     */
    private float asteroidMaxXAxis;
    private float asteroidMaxYAxis;
    private float asteroidMaxZAxis;
    private float asteroidMinXAxis;
    private float asteroidMinYAxis;
    private float asteroidMinZAxis;

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

    public AsteroidFieldControllerPersistance Serialize()
    {
        List<AsteroidControllerPersistance> asteroidPersistances = new List<AsteroidControllerPersistance>();

        foreach (KeyValuePair<ResourceType, HashSet<GameObject>> keyValuePair in _asteroids)
        {
            foreach (GameObject asteroid in keyValuePair.Value)
            {
                asteroidPersistances.Add(asteroid.GetComponent<AsteroidController>().Serialize());
            }
        }

        return new AsteroidFieldControllerPersistance(asteroidFieldAsteroidSettings.Serialize(), asteroidPersistances, initialized, GetComponent<MapObject>().Serialize(), size);
    }

    public ISerializable<AsteroidFieldControllerPersistance> SetObject(AsteroidFieldControllerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    public GameObject SpawnAsteroid(ResourceType resourceType, Vector3 position, bool setActive)
    {
        HashSet<GameObject> asteroidsOfType = _asteroids[resourceType];

        if (asteroidsOfType.Count >= asteroidFieldAsteroidSettings.asteroidTypeQuantity[resourceType]) //Checks if there is "room" for the new asteroid
        {
            throw new System.Exception("Not enough room for the new asteroid");
        }
        else
        {
            GameObject asteroid = Spawner.Instance.SpawnAsteroid(this, resourceType, (int)asteroidFieldAsteroidSettings.resourceQuantityOfResourceType[resourceType], position, false);
            AddAsteroid(resourceType, asteroid);
            asteroid.SetActive(setActive);

            return asteroid;
        }
    }

    public void SpawnAsteroidsOnField(bool destroyCurrentAsteroids)
    {
        if (destroyCurrentAsteroids)
        {
            foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
            {
                _asteroids[resourceType].Clear();
            }

            AsteroidController[] asteroidControllers = GetComponentsInChildren<AsteroidController>();

            foreach (AsteroidController asteroidController in asteroidControllers)
            {
                Destroy(asteroidController.gameObject);
            }
        }

        StartCoroutine(FieldSpawnerEnumerator());
        initialized = true;
    }

    private void AddAsteroid(ResourceType resourceType, GameObject asteroid)
    {
        _asteroids[resourceType].Add(asteroid);
        asteroid.GetComponent<AsteroidController>().destroyObservers += OnAsteroidDestroy;
    }

    private IEnumerator FieldSpawnerEnumerator()
    {
        foreach (KeyValuePair<ResourceType, uint> keyValuePair in asteroidFieldAsteroidSettings.asteroidTypeQuantity)
        {
            for (uint i = 0; i < keyValuePair.Value; i++)
            {
                System.Random random = new System.Random();

                float positionX = (float)random.NextDouble() * (asteroidMaxXAxis - asteroidMinXAxis) + asteroidMinXAxis;
                float positionY = (float)random.NextDouble() * (asteroidMaxYAxis - asteroidMinYAxis) + asteroidMinYAxis;
                float positionZ = (float)random.NextDouble() * (asteroidMaxZAxis - asteroidMinZAxis) + asteroidMinZAxis;

                Vector3 position = new Vector3(positionX, positionY, positionZ);
                SpawnAsteroid(keyValuePair.Key, position, true);
                yield return null;
            }
        }
    }

    private void OnAsteroidDestroy(ResourceType resourceType, GameObject asteroid)
    {
        _asteroids[resourceType].Remove(asteroid);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void OnEnable()
    {
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            _asteroids.Add(resourceType, new HashSet<GameObject>());
        }

        asteroidMinXAxis = transform.position.x - size.x / 2;
        asteroidMaxXAxis = transform.position.x + size.x / 2;

        asteroidMinYAxis = transform.position.y - size.y / 2;
        asteroidMaxYAxis = transform.position.y + size.y / 2;

        asteroidMinZAxis = transform.position.z - size.z / 2;
        asteroidMaxZAxis = transform.position.z + size.z / 2;

        AsteroidController[] asteroidControllers = GetComponentsInChildren<AsteroidController>();

        foreach (AsteroidController asteroidController in asteroidControllers)
        {
            ResourceType resourceType = asteroidController.resourceType;

            _asteroids[resourceType].Add(asteroidController.gameObject);
        }
    }
}