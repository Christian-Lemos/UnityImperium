using Imperium.Economy;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class ResourcePersistance
    {
        public int quantity;
        public ResourceType resourceType;

        public ResourcePersistance(ResourceType resourceType, int quantity)
        {
            this.quantity = quantity;
            this.resourceType = resourceType;
        }
    }
}