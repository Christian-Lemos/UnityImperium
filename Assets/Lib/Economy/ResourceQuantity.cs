namespace Imperium.Economy
{
    [System.Serializable]
    public class ResourceQuantity
    {
        public int quantity;
        public ResourceType resourceType;

        public ResourceQuantity(int quantity, ResourceType resourceType)
        {
            this.quantity = quantity;
            this.resourceType = resourceType;
        }
    }
}