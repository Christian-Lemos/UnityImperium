using Imperium.Economy;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class AsteroidControllerPersistance
    {
        public MapObjectPersitance mapObjectPersitance;
        public int resourceQuantity;
        public ResourceType resourceType;

        public AsteroidControllerPersistance(MapObjectPersitance mapObjectPersitance, int resourceQuantity, ResourceType resourceType)
        {
            this.mapObjectPersitance = mapObjectPersitance;
            this.resourceQuantity = resourceQuantity;
            this.resourceType = resourceType;
        }
    }
}