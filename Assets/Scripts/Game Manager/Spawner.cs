using Imperium.Economy;
using Imperium.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public GameObject asteroidFieldPreab;
    public GameObject[] asteroidsPrefabs;
    public long nextId = 1;
    public ShipAssosiation[] ship_assosiations;

    public StationAssosiation[] station_assosiations;
    public Dictionary<ShipType, GameObject> true_ship_associations = new Dictionary<ShipType, GameObject>();

    public Dictionary<StationType, GameObject> true_station_associations = new Dictionary<StationType, GameObject>();

    public GameObject SpawnAsteroid(long id, AsteroidFieldController asteroidFieldController, ResourceType resourceType, int resourceQuantity, Vector3 position, bool setActive)
    {
        System.Random random = new System.Random();

        GameObject prefab = asteroidsPrefabs[random.Next(0, asteroidsPrefabs.Length)]; //Picks a random prefab

        Vector3 prefabScale = prefab.transform.localScale;
        Vector3 fieldScale = asteroidFieldController.transform.localScale;

        GameObject asteroid = Instantiate(prefab, position, Quaternion.identity, asteroidFieldController.transform);

        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.resourceType = resourceType;
        asteroidController.ResourceQuantity = resourceQuantity;

        asteroid.SetActive(setActive);

        asteroid.GetComponent<MapObject>().id = id;

        return asteroid;
    }

    public GameObject SpawnAsteroid(AsteroidFieldController asteroidFieldController, ResourceType resourceType, int resourceQuantity, Vector3 position, bool setActive)
    {
        GameObject asteroid = SpawnAsteroid(CreateID(), asteroidFieldController, resourceType, resourceQuantity, position, setActive);
        SetMapObjectChildrenID(asteroid);
        return asteroid;
    }

    public GameObject SpawnAsteroidField(long id, AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings, Vector3 position, Vector3 size, bool setActive)
    {
        GameObject field = Instantiate(asteroidFieldPreab, position, Quaternion.identity);
        AsteroidFieldController asteroidFieldController = field.GetComponent<AsteroidFieldController>();
        asteroidFieldController.asteroidFieldAsteroidSettings = asteroidFieldAsteroidSettings;
        asteroidFieldController.size = size;

        field.GetComponent<MapObject>().id = id;

        field.SetActive(setActive);

        return field;
    }

    public GameObject SpawnAsteroidField(AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings, Vector3 position, Vector3 size, bool setActive)
    {
        GameObject field = SpawnAsteroidField(CreateID(), asteroidFieldAsteroidSettings, position, size, setActive);
        SetMapObjectChildrenID(field);
        return field;
    }

    public GameObject SpawnBullet(long id, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(prefab, position, rotation);
        bullet.GetComponent<MapObject>().id = id;
        return bullet;
    }

    public GameObject SpawnBullet(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject bullet = SpawnBullet(CreateID(), prefab, position, rotation);
        SetMapObjectChildrenID(bullet);
        return bullet;
    }

    public GameObject SpawnShip(long id, ShipType type, int player, Vector3 position, Quaternion rotation)
    {
        if (!PlayerDatabase.Instance.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = true_ship_associations[type];
        if (prefab == null)
        {
            throw new System.Exception("Ship type not supported");
        }
        else
        {
            GameObject newShip = Instantiate(prefab, position, rotation);
            PlayerDatabase.Instance.AddToPlayer(newShip, player);
            newShip.name += " " + player;

            newShip.GetComponent<MapObject>().id = id;
            newShip.SetActive(true);
            return newShip;
        }
    }

    public GameObject SpawnShip(ShipType type, int player, Vector3 position, Quaternion rotation)
    {
        GameObject ship = SpawnShip(CreateID(), type, player, position, rotation);
        SetMapObjectChildrenID(ship);
        return ship;
    }

    public GameObject SpawnStation(long id, StationType type, int player, Vector3 position, Quaternion rotation, int constructionProgress)
    {
        if (!PlayerDatabase.Instance.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = true_station_associations[type];

        if (prefab == null)
        {
            throw new System.Exception("Station type not supported");
        }
        else
        {
            GameObject newStation = Instantiate(prefab, position, rotation);
            PlayerDatabase.Instance.AddToPlayer(newStation, player);
            newStation.GetComponent<StationController>().constructed = constructionProgress == 100;
            newStation.GetComponent<StationController>().constructionProgress = constructionProgress;
            newStation.SetActive(true);
            newStation.name += " " + player;

            newStation.GetComponent<MapObject>().id = id;

            return newStation;
        }
    }

    public GameObject SpawnStation(StationType type, int player, Vector3 position, Quaternion rotation, int constructionProgress)
    {
        GameObject station = SpawnStation(CreateID(), type, player, position, rotation, constructionProgress);
        SetMapObjectChildrenID(station);
        return station;
    }

    private void Awake()
    {
        for (int i = 0; i < ship_assosiations.Length; i++)
        {
            true_ship_associations.Add(ship_assosiations[i].shipType, ship_assosiations[i].prefab);
        }
        for (int i = 0; i < station_assosiations.Length; i++)
        {
            true_station_associations.Add(station_assosiations[i].stationType, station_assosiations[i].prefab);
        }

        Instance = this;
    }

    public long CreateID()
    {
        long id = nextId;
        nextId++;
        return id;
    }

    public void SetMapObjectChildrenID(GameObject @object)
    {
        MapObject[] mapObjects = @object.GetComponentsInChildren<MapObject>();
        foreach (MapObject mapObject in mapObjects)
        {
            mapObject.id = CreateID();
        }
    }

    [System.Serializable]
    public struct ShipAssosiation
    {
        public GameObject prefab;
        public ShipType shipType;
    }

    [System.Serializable]
    public struct StationAssosiation
    {
        public GameObject prefab;
        public StationType stationType;
    }
}