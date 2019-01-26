using Imperium.Enum;
using System;
using System.Collections.Generic;

namespace Imperium.Economy
{
    [System.Serializable]
    public class ResourceStorage
    {
        public ResourceStorage(uint maximumResourcesStorage)
        {
            MaximumResourcesStorage = maximumResourcesStorage;
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

        public uint MaximumResourcesStorage { get; set; }
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
            uint remainingStorage = MaximumResourcesStorage;
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
    }
}