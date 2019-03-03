using Imperium.Enum;
using System.Collections.Generic;
using UnityEngine;
using Imperium.MapObjects;
public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public ShipAssosiation[] ship_assosiations;

    public StationAssosiation[] station_assosiations;

    public GameObject[] asteroidsPrefabs;

    public GameObject asteroidFieldPreab;

    public Dictionary<ShipType, GameObject> true_ship_associations = new Dictionary<ShipType, GameObject>();

    public Dictionary<StationType, GameObject> true_station_associations = new Dictionary<StationType, GameObject>();

    public GameObject SpawnShip(ShipType type, int player, Vector3 position, Quaternion rotation)
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
            return newShip;
        }
    }

    public GameObject SpawnStation(StationType type, int player, Vector3 position, Quaternion rotation, int constructionProgress)
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
            return newStation;
        }
    }

    public GameObject SpawnAsteroid(AsteroidFieldController asteroidFieldController, ResourceType resourceType, int resourceQuantity, Vector3 position, bool setActive)
    {
        

        System.Random random = new System.Random();

        GameObject prefab = asteroidsPrefabs[random.Next(0, asteroidsPrefabs.Length)]; //Picks a random prefab
        
        Vector3 prefabScale = prefab.transform.localScale;
        Vector3 fieldScale = asteroidFieldController.transform.localScale;

        GameObject asteroid = Instantiate(prefab, position, Quaternion.identity, asteroidFieldController.transform);
        asteroid.transform.localScale = new Vector3(prefabScale.x / fieldScale.x, prefabScale.y / fieldScale.y, prefabScale.z / fieldScale.z);

        MeshRenderer meshRenderer = asteroid.GetComponentInChildren<MeshRenderer>(); 
        Vector3 rendererScale= meshRenderer.transform.localScale;
        //meshRenderer.transform.localScale = new Vector3(rendererScale.x * asteroid.transform.localScale.x,rendererScale.y * asteroid.transform.localScale.y,rendererScale.z * asteroid.transform.localScale.z);
        //float lowestScale = prefab.transform.localScale.x / asteroidFieldController.transform.localScale.x;

       
       // Vector3 scaledVector = new Vector3(prefabScale.x * fieldScale.x, prefabScale.y * fieldScale.y, prefabScale.z * fieldScale.z);
       // float lcm = determineLCM(scaledVector.x, determineLCM(scaledVector.y, scaledVector.z));
       // Debug.Log(scaledVector);
       // Debug.Log(lcm);

       // Vector3 lcmVector = new Vector3(lcm / scaledVector.x , lcm / scaledVector.y , lcm/ scaledVector.z);
        //Debug.Log(lcmVector);

       // asteroid.transform.localScale = new Vector3(prefabScale.x / lcmVector.x, prefabScale.y / lcmVector.y, prefabScale.z / lcmVector.z);

        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.resourceType = resourceType;
        asteroidController.ResourceQuantity = resourceQuantity;

        asteroid.SetActive(setActive);

        return asteroid;
    }

    public GameObject SpawnAsteroidField(AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings, Vector3 position, Vector3 size, bool setActive)
    {
        GameObject field = Instantiate(asteroidFieldPreab, position, Quaternion.identity);
        AsteroidFieldController asteroidFieldController = field.GetComponent<AsteroidFieldController>();
        asteroidFieldController.asteroidFieldAsteroidSettings = asteroidFieldAsteroidSettings;
        asteroidFieldController.size = size;

        field.SetActive(setActive);

        return field;
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


    public static float determineLCM(float a, float b)
    {
        float num1, num2;
        if (a > b)
        {
            num1 = a; num2 = b;
        }
        else
        {
            num1 = b; num2 = a;
        }

        for (int i = 1; i < num2; i++)
        {
            if ((num1 * i) % num2 == 0)
            {
                return i * num1;
            }
        }
        return num1 * num2;
    }

}