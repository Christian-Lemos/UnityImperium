using Imperium.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class StationConstructionManager : MonoBehaviour
{
    public Dictionary<GameObject, float> constructionProgression;
    public static StationConstructionManager Instance { get; private set; }

    public void AddStationConstruction(StationType stationType, Vector3 position, Quaternion rotation, int player)
    {
        Spawner.Instance.SpawnStation(stationType, player, position, rotation, 0);
    }

    public void Awake()
    {
        Instance = this;
    }
}