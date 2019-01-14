using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Economy;
using Imperium.Enum;
public class Constructor : MonoBehaviour
{
    public Spawner spawner;

    [System.Serializable]
    public class ShipConstruction : Construction<ShipType>
    {
        public ShipConstruction(ShipType constructionType, int constructionTime, List<ResourceCost> resourceCosts) : base(constructionType, constructionTime, resourceCosts)
        {

        }
    }

    public List<ShipConstruction> ShipConstructions;

    public Vector3 relativeConstructionSpawn;

    private List<ShipConstruction> shipConstructionsQueue;


    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        shipConstructionsQueue = new List<ShipConstruction>();
        StartCoroutine("ConstructionIEnumerator");
    }
    public void BuildShip(ShipType type)
    {
        foreach(ShipConstruction shipConstruction in ShipConstructions)
        {
            if(shipConstruction.ConstructionType == type)
            {
                shipConstructionsQueue.Add(new ShipConstruction(type, shipConstruction.ConstructionTime, shipConstruction.ResourceCosts));
                return;
            }
        }
        throw new System.Exception("This ship type can't be constructed");
    }

   

    private IEnumerator ConstructionIEnumerator()
    {
        while(true)
        {
            try
            {
                if (shipConstructionsQueue[0] != null)
                {
                    Debug.Log(shipConstructionsQueue[0].ConstructionTime);
                    if (ShipConstructions[0].ConstructionTime <= 1)
                    {
                        SpawnShipConstruction(shipConstructionsQueue[0]);
                        shipConstructionsQueue.RemoveAt(0);
                    }
                    else
                    {
                        shipConstructionsQueue[0].ConstructionTime--;
                    }
                }
            }
            catch
            {

            }
            

            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnShipConstruction(ShipConstruction shipConstruction)
    {
        int player = PlayerDatabase.INSTANCE.GetObjectPlayer(this.gameObject);
        Vector3 thisPosition = this.gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(thisPosition.x + relativeConstructionSpawn.x, thisPosition.y + relativeConstructionSpawn.y, thisPosition.z + relativeConstructionSpawn.z);
        spawner.SpawnShip(shipConstruction.ConstructionType, player, spawnPosition, Quaternion.identity);
    }


}
