using System;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium.Economy
{
    public class ResourceStorage
    {
        public ResourceStorage(uint maximumResourcesStorage)
        {
            this.maximumResourcesStorage = maximumResourcesStorage;
            Storage = new Dictionary<ResourceType, uint>();
            Array values = System.Enum.GetValues(typeof(ResourceType));

            foreach (ResourceType resourceType in values)
            {
                Storage.Add(resourceType, 0);
            }

            
        }

        public ResourceStorage(uint maximumResourcesStorage, Dictionary<ResourceType, uint> storage) : this(maximumResourcesStorage)
        {
            Storage = storage;
        }

        public uint maximumResourcesStorage;
        public Dictionary<ResourceType, uint> Storage { get; set; }

        public void Add(ResourceType resourceType, uint quantity)
        {
            if (HasSuficientStorage(quantity))
            {
                Storage[resourceType] += quantity;
            }
        }

        public uint GetRemainingStorage()
        {
            uint remainingStorage = maximumResourcesStorage;
            foreach (KeyValuePair<ResourceType, uint> keyValue in Storage)
            {
                remainingStorage -= keyValue.Value;
            }
            return remainingStorage;
        }

        public bool HasSuficientStorage(uint quantity)
        {
            return GetRemainingStorage() >= quantity;
        }

        public bool IsEmpty()
        {
            return GetRemainingStorage() == maximumResourcesStorage;
        }

        public bool IsFull()
        {
            return GetRemainingStorage() == 0;
        }

        public void Remove(ResourceType resourceType, uint quantity)
        {
            int finalValue = (int)Storage[resourceType];

            if (finalValue < 0)
            {
                Storage[resourceType] = 0;
            }
            else
            {
                Storage[resourceType] -= (uint)finalValue;
            }
        }

        public List<ResourceQuantity> ToResourceQuantities()
        {
            List<ResourceQuantity> resourceQuantities = new List<ResourceQuantity>();

            foreach(KeyValuePair<ResourceType, uint> keyValuePair in Storage)
            {
                resourceQuantities.Add(new ResourceQuantity((int) keyValuePair.Value, keyValuePair.Key));
            }
            return resourceQuantities;
        }
    }
}