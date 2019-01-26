using Imperium.Enum;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class ResourcePersistance
    {
        public int Quantity;
        public ResourceType ResourceType;

        public ResourcePersistance(ResourceType resourceType, int quantity)
        {
            ResourceType = resourceType;
            Quantity = quantity;
        }
    }
}