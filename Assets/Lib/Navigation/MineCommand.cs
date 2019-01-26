using UnityEngine;
using System.Collections.Generic;
using Imperium.Enum;

namespace Imperium.Navigation
{
    public class MineCommand : FleetCommand
    {

        private ResourceStorageController resourceStorageController;
        private MineController mineController;
        private AsteroidController asteroidController;
        private int player;

        private GameObject deliveryObject = null;

        private bool isDelivering;

        public MineCommand(GameObject source, GameObject target, ShipMovement shipMovement) : base(source, target, shipMovement)
        {
            resourceStorageController = source.GetComponent<ResourceStorageController>();
            mineController = source.GetComponent<MineController>();
            asteroidController = target.GetComponent<AsteroidController>();
            base.destinationOffset = 2f;
            player = PlayerDatabase.Instance.GetObjectPlayer(source);
            Debug.Log(target);
        }

        public override void ExecuteCommand()
        {
            if(resourceStorageController.resourceStorage.GetRemainingStorage() > 0 && asteroidController != null) // It needs to moves to the asteroid
            {
                isDelivering = false;
                base.destination = base.target.transform.position;
                float distance = Vector3.Distance(base.destination, base.source.transform.position);

                if (distance > base.destinationOffset)
                {
                    shipMovement.MoveToPosition(base.destination);
                    
                }
                else
                {
                    mineController.StartMining(target);
                }
            }
            else
            {
                isDelivering = true;
                mineController.StopMining();
                
                if(deliveryObject == null)
                {
                    ICollection<GameObject> playerObjects = PlayerDatabase.Instance.GetObjects(player);

                    foreach(GameObject @object in playerObjects)
                    {
                        if(!@object.Equals(source) && @object.GetComponent<ShipController>().shipType == Enum.ShipType.MotherShip)
                        {
                            deliveryObject = @object;
                            break;
                        }
                    }
                }
                
                
                if(deliveryObject != null)
                {
                    
                    base.destination = deliveryObject.transform.position;
                    float distance = Vector3.Distance(base.destination, base.source.transform.position);

                    if (distance > base.destinationOffset)
                    {
                        shipMovement.MoveToPosition(base.destination);
                    }
                    else
                    {
                        deliveryObject = null;
                        Dictionary<ResourceType, uint> storage = resourceStorageController.resourceStorage.Storage;

                        List<ResourceType> keys = new List<ResourceType> (storage.Keys);

                        foreach (ResourceType key in keys)
                        {
                            PlayerDatabase.Instance.AddResourcesToPlayer(key, (int) storage[key], player);
                            storage[key] = 0;
                        }
                        isDelivering = false;
                    }
                }   
            }   
        }

        public override bool IsFinished()
        {
            return asteroidController == null && resourceStorageController.resourceStorage.IsEmpty();
        }

        public override void OnRemoved()
        {
            mineController.StopMining();
            Debug.Log(resourceStorageController.resourceStorage.IsEmpty());
            Debug.Log("Final storage: " + resourceStorageController.resourceStorage.GetRemainingStorage());
        }
    }
}