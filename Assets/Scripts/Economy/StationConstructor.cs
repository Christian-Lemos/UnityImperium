using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Economy;
using Imperium.Enum;
public class StationConstructor : MonoBehaviour {


    public List<StationConstruction> stationConstructions;
    public int ConstructionRate;
    public bool Building;

    private IEnumerator buildingCoroutine;


    public void BuildStation(StationType type, Vector3 position)
    {
        foreach (StationConstruction stationConstruction in stationConstructions)
        {
            if (stationConstruction.stationType == type)
            {
                int player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
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

                GameObject station = Spawner.Instance.SpawnStation(stationConstruction.stationType, player, position, Quaternion.identity, 1);
                this.GetComponent<ShipController>().BuildStation(station);
                return;
            }
        }
        throw new System.Exception("This ship type can't be constructed");
    }

    public void StartBuilding(GameObject station)
    {
        if(this.Building)
        {
            StopCoroutine(this.buildingCoroutine);
        }
        
        this.buildingCoroutine = BuildingEnumerator(station.GetComponent<StationController>());
        this.Building = true;
        StartCoroutine(this.buildingCoroutine);
    }

    public void StopBuilding()
    {
        StopCoroutine(this.buildingCoroutine);
        this.Building = false;
    }


    private IEnumerator BuildingEnumerator(StationController stationController)
    {
        while(stationController.constructed == false)
        {
            
            yield return new WaitForSeconds(1f);
            stationController.AddConstructionProgress(this.ConstructionRate);
        }
        this.Building = false;
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
