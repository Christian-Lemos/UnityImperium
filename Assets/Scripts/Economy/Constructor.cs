using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Economy;
using Imperium.Enum;
public class Constructor : MonoBehaviour
{
    public Spawner spawner;

    

    public List<ConstructionManager.ShipConstruction> ShipConstructions;

    public Vector3 relativeConstructionSpawn;


    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
    }
    public void BuildShip(ShipType type)
    {
        foreach(ConstructionManager.ShipConstruction shipConstruction in ShipConstructions)
        {
            if(shipConstruction.ConstructionType == type)
            {
                int player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
                Dictionary<ResourceType, int> resources = GetShipConstructionResources(shipConstruction);
                Dictionary<ResourceType, int> playerResources = PlayerDatabase.Instance.GetPlayerResources(player);

                foreach (KeyValuePair<ResourceType, int> entry in resources)
                {

                    if(playerResources[entry.Key] < entry.Value)
                    {
                        throw new System.Exception("Not Enough " + new Resource(entry.Key).Name);
                    }
                    
                    // do something with entry.Value or entry.Key
                }

                foreach (KeyValuePair<ResourceType, int> entry in resources)
                {
                    PlayerDatabase.Instance.AddResourcesToPlayer(entry.Key, -entry.Value, player);
                }

                ConstructionManager.Instance.ScheduleShipConstruction(this, shipConstruction);
                return;
            }   
        }
        throw new System.Exception("This ship type can't be constructed");
    }


    private Dictionary<ResourceType, int> GetShipConstructionResources(ConstructionManager.ShipConstruction construction)
    {
        Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

        for (int i = 0; i < construction.ResourceCosts.Count; i++)
        {
            if (!resources.ContainsKey(construction.ResourceCosts[i].resourceType))
            {
                resources[construction.ResourceCosts[i].resourceType] = 0;
            }
            
            resources[construction.ResourceCosts[i].resourceType] += construction.ResourceCosts[i].quantity;
        }

        return resources;

    }
   


}
