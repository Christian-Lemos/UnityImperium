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
                //shipConstructionsQueue.Add(new ShipConstruction(type, shipConstruction.ConstructionTime, shipConstruction.ResourceCosts));
                ConstructionManager.Instance.ScheduleShipConstruction(this, shipConstruction);
                return;
            }
        }
        throw new System.Exception("This ship type can't be constructed");
    }

   


}
