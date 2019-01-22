using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
using Imperium.Economy;
public class StationConstructionManager : MonoBehaviour {

    public static StationConstructionManager Instance { get; private set; }
    public Dictionary<GameObject, float> constructionProgression;

    public void Awake()
    {
        Instance = this;
    }

    public void AddStationConstruction(StationType stationType, Vector3 position, Quaternion rotation, int player)
    {
        Spawner.Instance.SpawnStation(stationType, player, position, rotation, 0);
        
    }


}
