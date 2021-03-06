﻿using Imperium;
using Imperium.Economy;
using Imperium.MapObjects;
using System;
using System.Collections.Generic;
using Imperium.AI;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public GameObject asteroidFieldPreab;
    public GameObject[] dummyAsteroidsPrefabs;
    public ShipAssociation[] dummyShipAssociations;
    public Dictionary<ShipType, GameObject> dummyShipDictionary = new Dictionary<ShipType, GameObject>();
    public StationAssociation[] dummyStationAssociation;
    public Dictionary<StationType, GameObject> dummyStationDictionary = new Dictionary<StationType, GameObject>();
    [NonSerialized]
    public long nextId = 1;
    public GameObject[] trueAsteroidsPrefabs;
    public ShipAssociation[] trueShipAssociations;
    public Dictionary<ShipType, GameObject> trueShipDictionary = new Dictionary<ShipType, GameObject>();
    public StationAssociation[] trueStationAssociation;
    public Dictionary<StationType, GameObject> trueStationDictionary = new Dictionary<StationType, GameObject>();

    private Dictionary<Player, HashSet<Action<GameObject>>> playerShipCreationObservers = new Dictionary<Player, HashSet<Action<GameObject>>>();


    public delegate void ShipSpawnMiddleware(ref GameObject instance);
    private HashSet<ShipSpawnMiddleware> shipSpawnMiddlewares = new HashSet<ShipSpawnMiddleware>();


    /*private abstract class MiddleWareList<T> 
    {
        private List<T> collection = new List<T>();
        public abstract void Execute();
    }

    private class ShipMiddleWareList : MiddleWareList<ShipSpawnMiddleware>
    {
        public override void Execute()
        {
            
        }
    }*/

    public void AddShipSpawnMiddleware(ShipSpawnMiddleware shipSpawnMiddleware)
    {
        shipSpawnMiddlewares.Add(shipSpawnMiddleware);
    }

    public void RemoveShipSpawnMiddleware(ShipSpawnMiddleware shipSpawnMiddleware)
    {
        shipSpawnMiddlewares.Remove(shipSpawnMiddleware);
    }

    public long CreateID()
    {
        long id = nextId;
        nextId++;
        return id;
    }

    public void ObserveShipCreation(Player player, Action<GameObject> action)
    {
        playerShipCreationObservers[player].Add(action);
    }

    public void RemoveObservationShipCreation(Player player, Action<GameObject> action)
    {
        playerShipCreationObservers[player].RemoveWhere((Action<GameObject> a) =>
        {
            return a.Equals(action);
        });
    }

    public void SetMapObjectChildrenID(GameObject @object)
    {
        MapObject[] mapObjects = @object.GetComponentsInChildren<MapObject>();
        MapObject parent = @object.GetComponent<MapObject>();

        foreach (MapObject mapObject in mapObjects)
        {
            if (!mapObject.Equals(parent))
            {
                mapObject.id = CreateID();
            }
        }
    }

    public GameObject SpawnAsteroid(long id, AsteroidFieldController asteroidFieldController, ResourceType resourceType, int resourceQuantity, Vector3 position, bool setActive)
    {
        return SpawnAsteroid(new System.Random().Next(0, trueAsteroidsPrefabs.Length), CreateID(), asteroidFieldController, resourceType, resourceQuantity, position, setActive);
    }

    public GameObject SpawnAsteroid(int prefabIndex, long id, AsteroidFieldController asteroidFieldController, ResourceType resourceType, int resourceQuantity, Vector3 position, bool setActive)
    {
        GameObject prefab = trueAsteroidsPrefabs[prefabIndex]; //Picks a random prefab

        Vector3 prefabScale = prefab.transform.localScale;
        Vector3 fieldScale = asteroidFieldController.transform.localScale;

        GameObject asteroid = Instantiate(prefab, position, Quaternion.identity, asteroidFieldController.transform);

        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.resourceType = resourceType;
        asteroidController.ResourceQuantity = resourceQuantity;
        asteroidController.prefabIndex = prefabIndex;

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

    public GameObject SpawnDummyAsteroid(int prefabIndex, ResourceType resourceType, int resourceQuantity, Vector3 position, Quaternion rotation, bool setActive)
    {
        GameObject prefab = dummyAsteroidsPrefabs[prefabIndex];

        GameObject createdAsteroid = Instantiate(prefab, position, rotation);

        DummyAsteroid dummyAsteroid = createdAsteroid.GetComponent<DummyAsteroid>();
        dummyAsteroid.resourceType = resourceType;
        dummyAsteroid.resourceQuantity = resourceQuantity;

        createdAsteroid.SetActive(setActive);

        return createdAsteroid;
    }

    public GameObject SpawnDummyAsteroid(ResourceType resourceType, int resourceQuantity, Vector3 position, Quaternion rotation, bool setActive)
    {
        return SpawnDummyAsteroid(new System.Random().Next(0, dummyAsteroidsPrefabs.Length), resourceType, resourceQuantity, position, rotation, setActive);
    }

    public GameObject SpawnDummyStation(StationType stationType, Station station, Vector3 position, Quaternion rotation, float constructionProgress, bool setActive)
    {
        GameObject prefab = dummyStationDictionary[stationType];
        GameObject createdStation = Instantiate(prefab, position, Quaternion.identity);

        DummyStation dummyStation = createdStation.GetComponent<DummyStation>();
        dummyStation.station = station;
        dummyStation.stationType = stationType;
        dummyStation.constructionProgress = constructionProgress;

        createdStation.SetActive(setActive);

        return createdStation;
    }

    public GameObject SpawnShip(long id, ShipType type, Player player, Vector3 position, Quaternion rotation, bool setActive)
    {
        if (!PlayerDatabase.Instance.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = trueShipDictionary[type];
        if (prefab == null)
        {
            throw new System.Exception("Ship type not supported");
        }
        else
        {
            GameObject newShip = Instantiate(prefab, position, rotation);
            //Ship ship = ShipFactory.getInstance().CreateShip(type);

            foreach(ShipSpawnMiddleware middleware in this.shipSpawnMiddlewares)
            {
                middleware.Invoke(ref newShip);
            }

            PlayerDatabase.Instance.AddObjectToPlayer(newShip, player);
            newShip.name += " " + player.Number;

            newShip.GetComponent<MapObject>().id = id;
            newShip.SetActive(setActive);

            AddAI(player, newShip);

            CallShipCreationObservers(player, newShip);

            return newShip;
        }
    }

    public GameObject SpawnShip(ShipType type, Player player, Vector3 position, Quaternion rotation, bool setActive)
    {
        GameObject ship = SpawnShip(CreateID(), type, player, position, rotation, setActive);
        SetMapObjectChildrenID(ship);
        return ship;
    }

    public GameObject SpawnStation(long id, StationType type, Player player, Vector3 position, Quaternion rotation, float constructionProgress, bool setActive)
    {
        if (!PlayerDatabase.Instance.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = trueStationDictionary[type];

        if (prefab == null)
        {
            throw new System.Exception("Station type not supported");
        }
        else
        {
            GameObject newStation = Instantiate(prefab, position, rotation);
            PlayerDatabase.Instance.AddObjectToPlayer(newStation, player);
            newStation.GetComponent<StationController>().Station = StationFactory.getInstance().CreateStation(type);
            newStation.GetComponent<StationController>().Constructed = constructionProgress >= 100;
            newStation.GetComponent<StationController>().constructionProgress = constructionProgress;
            newStation.SetActive(setActive);
            newStation.name += " " + player;

            newStation.GetComponent<MapObject>().id = id;

            return newStation;
        }
    }

    public GameObject SpawnStation(StationType type, Player player, Vector3 position, Quaternion rotation, float constructionProgress, bool setActive)
    {
        GameObject station = SpawnStation(CreateID(), type, player, position, rotation, constructionProgress, setActive);
        SetMapObjectChildrenID(station);
        return station;
    }


    private void Awake()
    {
        for (int i = 0; i < SceneManager.Instance.currentGameSceneData.players.Count; i++)
        {
            playerShipCreationObservers.Add(SceneManager.Instance.currentGameSceneData.players[i].player, new HashSet<Action<GameObject>>());
        }

        for (int i = 0; i < trueShipAssociations.Length; i++)
        {
            trueShipDictionary.Add(trueShipAssociations[i].shipType, trueShipAssociations[i].prefab);
        }
        for (int i = 0; i < trueStationAssociation.Length; i++)
        {
            trueStationDictionary.Add(trueStationAssociation[i].stationType, trueStationAssociation[i].prefab);
        }

        for (int i = 0; i < dummyShipAssociations.Length; i++)
        {
            dummyShipDictionary.Add(dummyShipAssociations[i].shipType, dummyShipAssociations[i].prefab);
        }
        for (int i = 0; i < dummyStationAssociation.Length; i++)
        {
            dummyStationDictionary.Add(dummyStationAssociation[i].stationType, dummyStationAssociation[i].prefab);
        }

        Instance = this;
    }

    private void CallShipCreationObservers(Player player, GameObject ship)
    {
        foreach (Action<GameObject> action in playerShipCreationObservers[player])
        {
            action.Invoke(ship);
        }
    }

    public bool AddAI(Player player, GameObject gameObject)
    {
        if(player.PlayerType == PlayerType.AI)
        {
            ShipController shipController = gameObject.GetComponent<ShipController>();
            if(shipController != null)
            {
                switch (shipController.shipType)
                {
                    case ShipType.Freighter:
                        gameObject.AddComponent<FreighterShipAI>();
                        return true;
                    case ShipType.Destroyer:
                        gameObject.AddComponent<MilitaryShipAI>();
                        return true;
                    case ShipType.Test:
                        gameObject.AddComponent<MilitaryShipAI>();
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    [System.Serializable]
    public struct ShipAssociation
    {
        public GameObject prefab;
        public ShipType shipType;
    }

    [System.Serializable]
    public struct StationAssociation
    {
        public GameObject prefab;
        public StationType stationType;
    }
}