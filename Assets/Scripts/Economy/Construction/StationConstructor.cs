using Imperium.Economy;
using Imperium.MapObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
[DisallowMultipleComponent]
public class StationConstructor : MonoBehaviour
{
    public bool building;
    public int constructionRate;
    public List<StationConstruction> stationConstructions;
    private IEnumerator buildingCoroutine;

    public GameObject BuildStation(StationType type, Vector3 position)
    {
        foreach (StationConstruction stationConstruction in stationConstructions)
        {
            if (stationConstruction.stationType == type)
            {
                int player = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
                Dictionary<ResourceType, int> resources = GetStationConstructionResources(stationConstruction);
                Dictionary<ResourceType, int> playerResources = PlayerDatabase.Instance.GetPlayerResources(player);

                foreach (KeyValuePair<ResourceType, int> entry in resources)
                {
                    if (playerResources[entry.Key] < entry.Value)
                    {
                        throw new System.Exception("Not Enough " + new Resource(entry.Key).Name);
                    }

                    // do something with entry.Value or entry.Key
                }

                foreach (KeyValuePair<ResourceType, int> entry in resources)
                {
                    PlayerDatabase.Instance.AddResourcesToPlayer(entry.Key, -entry.Value, player);
                }

                return Spawner.Instance.SpawnStation(stationConstruction.stationType, player, position, Quaternion.identity, 1, true);
            }
        }
        throw new System.Exception("This station type can't be constructed");
    }

    public void StartBuilding(GameObject station)
    {
        if (building)
        {
            StopCoroutine(buildingCoroutine);
        }

        buildingCoroutine = BuildingEnumerator(station.GetComponent<StationController>());
        building = true;
        StartCoroutine(buildingCoroutine);
    }

    public void StopBuilding()
    {
        if (building)
        {
            StopCoroutine(buildingCoroutine);
            building = false;
        }
    }

    private IEnumerator BuildingEnumerator(StationController stationController)
    {
        while (stationController.constructed == false)
        {
            yield return new WaitForSeconds(1f);
            stationController.AddConstructionProgress(constructionRate);
        }
        building = false;
    }

    private Dictionary<ResourceType, int> GetStationConstructionResources(StationConstruction construction)
    {
        Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

        for (int i = 0; i < construction.resourceCosts.Count; i++)
        {
            if (!resources.ContainsKey(construction.resourceCosts[i].resourceType))
            {
                resources[construction.resourceCosts[i].resourceType] = 0;
            }

            resources[construction.resourceCosts[i].resourceType] += construction.resourceCosts[i].quantity;
        }

        return resources;
    }
}