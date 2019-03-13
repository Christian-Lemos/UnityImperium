using Imperium.Economy;
using System.Collections.Generic;

namespace Imperium.Persistence.ShipModules
{
    [System.Serializable]
    public class ResourceStoragePersistance
    {
        public uint maximumStorage;
        public List<ResourceQuantity> resourceQuantities;

        public ResourceStoragePersistance(uint maximumStorage, List<ResourceQuantity> resourceQuantities)
        {
            this.maximumStorage = maximumStorage;
            this.resourceQuantities = resourceQuantities;
        }
    }
}