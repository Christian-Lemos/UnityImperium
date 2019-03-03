using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Imperium.Enum;
namespace Imperium.MapObjects
{
    public class AsteroidFieldAsteroidSettings
    {
        public Dictionary<ResourceType, uint> asteroidTypeQuantity;
        public Dictionary<ResourceType, uint> resourceQuantityOfResourceType; //How much resources will each resource type have at it's respective asteroid.

        public AsteroidFieldAsteroidSettings(Dictionary<ResourceType, uint> asteroidTypeQuantity, Dictionary<ResourceType, uint> resourceQuantityOfResourceType)
        {
            this.asteroidTypeQuantity = asteroidTypeQuantity;
            this.resourceQuantityOfResourceType = resourceQuantityOfResourceType;
        }

        public static AsteroidFieldAsteroidSettings CreateDefaultSettings()
        {
            Dictionary<ResourceType, uint> resourceQuantityOfResourceType = new Dictionary<ResourceType, uint>()
            {
                {ResourceType.Metal,  500},
                {ResourceType.Crystal, 300}
            };

            Dictionary<ResourceType, uint> asteroidTypeQuantity = new Dictionary<ResourceType, uint>()
            {
                {ResourceType.Metal,  3},
                {ResourceType.Crystal, 2}
            };

            return new AsteroidFieldAsteroidSettings(asteroidTypeQuantity, resourceQuantityOfResourceType);


        }
    }
}

