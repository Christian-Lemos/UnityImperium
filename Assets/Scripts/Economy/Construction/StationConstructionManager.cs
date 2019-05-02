using Imperium;
using Imperium.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class StationConstructionManager : MonoBehaviour
{
    public Dictionary<GameObject, float> constructionProgression;
    public static StationConstructionManager Instance { get; private set; }

    public void AddStationConstruction(StationType stationType, Vector3 position, Quaternion rotation, Player player)
    {
        Spawner.Instance.SpawnStation(stationType, player, position, rotation, 0, true);
    }

    public void Awake()
    {
        Instance = this;
    }
}