using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Economy;
using Imperium.Enum;
public class ConstructionManager : MonoBehaviour {




    [System.Serializable]
    public class ShipConstruction : Construction<ShipType>
    {
        public ShipConstruction(ShipType constructionType, int constructionTime, List<ResourceCost> resourceCosts) : base(constructionType, constructionTime, resourceCosts)
        {

        }
    }

    public class OnGoingShipConstruction
    {
        public Constructor constructor;
        public List<ShipConstruction> ShipConstructions;
        public IEnumerator enumerator;

        public OnGoingShipConstruction(Constructor constructor)
        {
            this.constructor = constructor;
            this.ShipConstructions = new List<ShipConstruction>();
        }
    }


    

    private Dictionary<Constructor, OnGoingShipConstruction> shipConstructions = new Dictionary<Constructor, OnGoingShipConstruction>();

    public static ConstructionManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void ScheduleShipConstruction(Constructor target, ShipConstruction shipConstruction)
    {
        ShipConstruction shipConstructionCopy = new ShipConstruction(shipConstruction.ConstructionType, shipConstruction.ConstructionTime, shipConstruction.ResourceCosts); //Makes a copy
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



    private IEnumerator ConstructionCoroutine(OnGoingShipConstruction onGoingShipConstruction)
    {
        while(onGoingShipConstruction.ShipConstructions.Count != 0)
        {
            Debug.Log("Count: " + onGoingShipConstruction.ShipConstructions.Count);
            Debug.Log("Time: " + onGoingShipConstruction.ShipConstructions[0].ConstructionTime);
            if (onGoingShipConstruction.ShipConstructions[0].ConstructionTime <= 0)
            {
                SpawnShipConstruction(onGoingShipConstruction.constructor, onGoingShipConstruction.ShipConstructions[0]);
                onGoingShipConstruction.ShipConstructions.RemoveAt(0);
            }
            else
            {
                onGoingShipConstruction.ShipConstructions[0].ConstructionTime--;
            }

            yield return new WaitForSeconds(1f);
        }

        shipConstructions.Remove(onGoingShipConstruction.constructor);


    }
    



    private void SpawnShipConstruction(Constructor constructor, ShipConstruction shipConstruction)
    {
        int player = PlayerDatabase.INSTANCE.GetObjectPlayer(constructor.gameObject);
        Vector3 thisPosition = constructor.gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(thisPosition.x + constructor.relativeConstructionSpawn.x, thisPosition.y + constructor.relativeConstructionSpawn.y, thisPosition.z + constructor.relativeConstructionSpawn.z);
        Spawner.Instance.SpawnShip(shipConstruction.ConstructionType, player, spawnPosition, Quaternion.identity);
    }
}
