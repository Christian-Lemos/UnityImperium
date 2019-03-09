using UnityEngine;
using System.Collections.Generic;
using Imperium.Economy;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class AsteroidFieldAsteroidSettingsPersistance
    {
        [System.Serializable]
        public struct ResourceNUint
        {
            public ResourceType resourceType;
            public uint quantity;

            public ResourceNUint(ResourceType resourceType, uint quantity)
            {
                this.resourceType = resourceType;
                this.quantity = quantity;
            }
        }

        public List<ResourceNUint> asteroidTypeQuantity;
        public List<ResourceNUint> resourceQuantityOfResourceType;

        public AsteroidFieldAsteroidSettingsPersistance(List<ResourceNUint> asteroidTypeQuantity, List<ResourceNUint> resourceQuantityOfResourceType)
        {
            this.asteroidTypeQuantity = asteroidTypeQuantity;
            this.resourceQuantityOfResourceType = resourceQuantityOfResourceType;
        }
    }
}
