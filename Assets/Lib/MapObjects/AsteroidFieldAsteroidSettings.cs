using Imperium.Economy;
using Imperium.Persistence;
using System.Collections.Generic;

namespace Imperium.MapObjects
{
    [System.Serializable]
    public class AsteroidFieldAsteroidSettings : ISerializable<AsteroidFieldAsteroidSettingsPersistance>
    {
        public Dictionary<ResourceType, uint> asteroidTypeQuantity = new Dictionary<ResourceType, uint>();
        public Dictionary<ResourceType, uint> resourceQuantityOfResourceType = new Dictionary<ResourceType, uint>(); //How much resources will each resource type have at it's respective asteroid.

        public AsteroidFieldAsteroidSettings()
        {
        }

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

        public AsteroidFieldAsteroidSettingsPersistance Serialize()
        {
            List<AsteroidFieldAsteroidSettingsPersistance.ResourceNUint> asteroidTypeQuantity = new List<AsteroidFieldAsteroidSettingsPersistance.ResourceNUint>();

            foreach (KeyValuePair<ResourceType, uint> keyValuePair in this.asteroidTypeQuantity)
            {
                asteroidTypeQuantity.Add(new AsteroidFieldAsteroidSettingsPersistance.ResourceNUint(keyValuePair.Key, keyValuePair.Value));
            }

            List<AsteroidFieldAsteroidSettingsPersistance.ResourceNUint> resourceQuantityOfResourceType = new List<AsteroidFieldAsteroidSettingsPersistance.ResourceNUint>();

            foreach (KeyValuePair<ResourceType, uint> keyValuePair in this.resourceQuantityOfResourceType)
            {
                resourceQuantityOfResourceType.Add(new AsteroidFieldAsteroidSettingsPersistance.ResourceNUint(keyValuePair.Key, keyValuePair.Value));
            }

            return new AsteroidFieldAsteroidSettingsPersistance(asteroidTypeQuantity, resourceQuantityOfResourceType);
        }

        public void SetObject(AsteroidFieldAsteroidSettingsPersistance serializedObject)
        {
            foreach (AsteroidFieldAsteroidSettingsPersistance.ResourceNUint resourceNUint in serializedObject.asteroidTypeQuantity)
            {
                asteroidTypeQuantity.Add(resourceNUint.resourceType, resourceNUint.quantity);
            }

            foreach (AsteroidFieldAsteroidSettingsPersistance.ResourceNUint resourceNUint in serializedObject.resourceQuantityOfResourceType)
            {
                resourceQuantityOfResourceType.Add(resourceNUint.resourceType, resourceNUint.quantity);
            }
        }
    }
}