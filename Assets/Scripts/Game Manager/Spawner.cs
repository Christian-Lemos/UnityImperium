using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
public class Spawner : MonoBehaviour {
    [System.Serializable]
    public struct ShipAssosiation
    {
        public ShipType shipType;
        public GameObject prefab;
    }
    public ShipAssosiation[] ship_assosiations;

    [System.Serializable]
    public struct StationAssosiation
    {
        public StationType stationType;
        public GameObject prefab;
    }
    public StationAssosiation[] station_assosiations;

    public Dictionary<ShipType, GameObject> true_ship_associations = new Dictionary<ShipType, GameObject>();
    public Dictionary<StationType, GameObject> true_station_associations = new Dictionary<StationType, GameObject>();


    public static Spawner Instance;

    private void Awake()
    {
        for(int i = 0; i < ship_assosiations.Length; i++)
        {
            true_ship_associations.Add(ship_assosiations[i].shipType, ship_assosiations[i].prefab);
        }
        for (int i = 0; i < station_assosiations.Length; i++)
        {
            true_station_associations.Add(station_assosiations[i].stationType, station_assosiations[i].prefab);
        }

        Instance = this;
    }


    public GameObject SpawnShip(ShipType type, int player, Vector3 position, Quaternion rotation)
    {
        if (!PlayerDatabase.Instance.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = true_ship_associations[type];
        if(prefab == null)
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
}
