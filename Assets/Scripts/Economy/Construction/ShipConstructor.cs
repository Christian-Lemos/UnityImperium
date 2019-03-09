using Imperium.Economy;
using Imperium.MapObjects;
using System.Collections.Generic;
using UnityEngine;

public class ShipConstructor : MonoBehaviour
{
    public Vector3 relativeConstructionSpawn;
    public List<ShipConstruction> ShipConstructions;

    public void BuildShip(ShipType type)
    {
        foreach (ShipConstruction shipConstruction in ShipConstructions)
        {
            if (shipConstruction.shipType == type)
            {
                int player = PlayerDatabase.Instance.GetObjectPlayer(gameObject);
                Dictionary<ResourceType, int> resources = GetShipConstructionResources(shipConstruction);
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

                ShipConstructionManager.Instance.ScheduleShipConstruction(this, shipConstruction);
                return;
            }
        }
        throw new System.Exception("This ship type can't be constructed");
    }

    private Dictionary<ResourceType, int> GetShipConstructionResources(ShipConstruction construction)
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