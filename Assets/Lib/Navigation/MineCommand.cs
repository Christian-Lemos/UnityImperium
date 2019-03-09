﻿using System.Collections.Generic;
using UnityEngine;
using Imperium.MapObjects;
using Imperium.Economy;
namespace Imperium.Navigation
{
    public class MineCommand : FleetCommand
    {
        private AsteroidController asteroidController;
        private GameObject deliveryObject = null;
        private MineController mineController;
        private int player;
        private ResourceStorageController resourceStorageController;

        public MineCommand(MapObject source, MapObject target) : base(source, target, CommandType.Mine)
        {
            resourceStorageController = source.GetComponent<ResourceStorageController>();
            mineController = source.GetComponent<MineController>();
            asteroidController = target.GetComponent<AsteroidController>();
            base.destinationOffset = 2f;
            player = PlayerDatabase.Instance.GetObjectPlayer(source.gameObject);
        }

        public override void ExecuteCommand()
        {
            if (resourceStorageController.resourceStorage.GetRemainingStorage() > 0 && asteroidController != null) // It needs to moves to the asteroid
            {
                base.destination = base.target.transform.position;
                float distance = Vector3.Distance(base.destination, base.source.transform.position);

                if (distance > base.destinationOffset)
                {
                    sourceShipController.MoveControl(base.destination);
                }
                else
                {
                    mineController.StartMining(target.gameObject);
                }
            }
            else
            {
                mineController.StopMining();

                if (deliveryObject == null)
                {
                    ICollection<GameObject> playerObjects = PlayerDatabase.Instance.GetObjects(player);

                    foreach (GameObject @object in playerObjects)
                    {
                        if (!@object.Equals(source) && @object.GetComponent<ShipController>().shipType == ShipType.MotherShip)
                        {
                            deliveryObject = @object;
                            break;
                        }
                    }
                }

                if (deliveryObject != null)
                {
                    base.destination = deliveryObject.transform.position;
                    float distance = Vector3.Distance(base.destination, base.source.transform.position);

                    if (distance > base.destinationOffset)
                    {
                        sourceShipController.MoveControl(base.destination);
                    }
                    else
                    {
                        deliveryObject = null;
                        Dictionary<ResourceType, uint> storage = resourceStorageController.resourceStorage.Storage;

                        List<ResourceType> keys = new List<ResourceType>(storage.Keys);

                        foreach (ResourceType key in keys)
                        {
                            PlayerDatabase.Instance.AddResourcesToPlayer(key, (int)storage[key], player);
                            storage[key] = 0;
                        }
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
        }
    }
}