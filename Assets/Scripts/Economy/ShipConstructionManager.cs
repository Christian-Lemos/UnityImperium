using Imperium.Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConstructionManager : MonoBehaviour
{
    private Dictionary<ShipConstructor, OnGoingShipConstruction> shipConstructions = new Dictionary<ShipConstructor, OnGoingShipConstruction>();

    public static ShipConstructionManager Instance { get; private set; }

    public void ScheduleShipConstruction(ShipConstructor target, ShipConstruction shipConstruction)
    {
        ShipConstruction shipConstructionCopy = new ShipConstruction(shipConstruction.shipType, shipConstruction.constructionTime, shipConstruction.resourceCosts); //Makes a copy
        if (!shipConstructions.ContainsKey(target))
        {
            OnGoingShipConstruction onGoingShipConstruction = new OnGoingShipConstruction(target);
            onGoingShipConstruction.ShipConstructions.Add(shipConstructionCopy);

            IEnumerator enumerator = ConstructionCoroutine(onGoingShipConstruction);
            onGoingShipConstruction.enumerator = enumerator;

            shipConstructions.Add(target, onGoingShipConstruction);

            StartCoroutine(enumerator);
        }
        else
        {
            shipConstructions[target].ShipConstructions.Add(shipConstructionCopy);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator ConstructionCoroutine(OnGoingShipConstruction onGoingShipConstruction)
    {
        while (onGoingShipConstruction.ShipConstructions.Count != 0)
        {
            if (onGoingShipConstruction.constructor == null)
            {
                break;
            }

            if (onGoingShipConstruction.ShipConstructions[0].constructionTime <= 0)
            {
                SpawnShipConstruction(onGoingShipConstruction.constructor, onGoingShipConstruction.ShipConstructions[0]);
                onGoingShipConstruction.ShipConstructions.RemoveAt(0);
            }
            else
            {
                onGoingShipConstruction.ShipConstructions[0].constructionTime--;
            }

            yield return new WaitForSeconds(1f);
        }

        shipConstructions.Remove(onGoingShipConstruction.constructor);
    }

    private void SpawnShipConstruction(ShipConstructor constructor, ShipConstruction shipConstruction)
    {
        int player = PlayerDatabase.Instance.GetObjectPlayer(constructor.gameObject);
        Vector3 thisPosition = constructor.gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(thisPosition.x + constructor.relativeConstructionSpawn.x, thisPosition.y + constructor.relativeConstructionSpawn.y, thisPosition.z + constructor.relativeConstructionSpawn.z);
        Spawner.Instance.SpawnShip(shipConstruction.shipType, player, spawnPosition, Quaternion.identity);
    }

    public class OnGoingShipConstruction
    {
        public ShipConstructor constructor;
        public IEnumerator enumerator;
        public List<ShipConstruction> ShipConstructions;

        public OnGoingShipConstruction(ShipConstructor constructor)
        {
            this.constructor = constructor;
            ShipConstructions = new List<ShipConstruction>();
        }
    }
}