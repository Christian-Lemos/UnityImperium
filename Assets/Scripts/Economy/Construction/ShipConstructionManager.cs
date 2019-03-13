using Imperium.Economy;
using Imperium.Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConstructionManager : MonoBehaviour, ISerializable<ShipConstructionManagerPersistance>
{
    private Dictionary<ShipConstructor, OnGoingShipConstruction> shipConstructions = new Dictionary<ShipConstructor, OnGoingShipConstruction>();

    public static ShipConstructionManager Instance { get; private set; }

    public void ScheduleShipConstruction(ShipConstructor target, ShipConstruction shipConstruction)
    {
        ShipConstruction shipConstructionCopy = new ShipConstruction(shipConstruction.shipType, shipConstruction.constructionTime, shipConstruction.resourceCosts); //Makes a copy
        if (!shipConstructions.ContainsKey(target))
        {
            OnGoingShipConstruction onGoingShipConstruction = new OnGoingShipConstruction(target);
            onGoingShipConstruction.shipConstructions.Add(shipConstructionCopy);

            IEnumerator enumerator = ConstructionCoroutine(onGoingShipConstruction);
            onGoingShipConstruction.enumerator = enumerator;

            shipConstructions.Add(target, onGoingShipConstruction);

            StartCoroutine(enumerator);
        }
        else
        {
            shipConstructions[target].shipConstructions.Add(shipConstructionCopy);
        }
    }

    public ShipConstructionManagerPersistance Serialize()
    {
        List<ShipConstructionManagerPersistance.Construction> constructions = new List<ShipConstructionManagerPersistance.Construction>();

        foreach (KeyValuePair<ShipConstructor, OnGoingShipConstruction> keyValuePair in shipConstructions)
        {
            constructions.Add(new ShipConstructionManagerPersistance.Construction(MapObject.GetID(keyValuePair.Key), keyValuePair.Value));
        }
        return new ShipConstructionManagerPersistance(constructions);
    }

    public ISerializable<ShipConstructionManagerPersistance> SetObject(ShipConstructionManagerPersistance serializedObject)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator ConstructionCoroutine(OnGoingShipConstruction onGoingShipConstruction)
    {
        while (onGoingShipConstruction.shipConstructions.Count != 0)
        {
            if (onGoingShipConstruction.constructor == null)
            {
                break;
            }

            if (onGoingShipConstruction.shipConstructions[0].constructionTime <= 0)
            {
                SpawnShipConstruction(onGoingShipConstruction.constructor, onGoingShipConstruction.shipConstructions[0]);
                onGoingShipConstruction.shipConstructions.RemoveAt(0);
            }
            else
            {
                onGoingShipConstruction.shipConstructions[0].constructionTime--;
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

    [System.Serializable]
    public class OnGoingShipConstruction
    {
        public ShipConstructor constructor;
        public IEnumerator enumerator;
        public List<ShipConstruction> shipConstructions;

        public long shipContructorID;

        public OnGoingShipConstruction(ShipConstructor constructor)
        {
            this.constructor = constructor;
            shipContructorID = constructor.gameObject.GetComponent<MapObject>().id;
            shipConstructions = new List<ShipConstruction>();
        }

        public OnGoingShipConstruction(long id)
        {
            shipContructorID = id;
            constructor = MapObject.FindByID(id).gameObject.GetComponent<ShipConstructor>();
            shipConstructions = new List<ShipConstruction>();
        }
    }
}