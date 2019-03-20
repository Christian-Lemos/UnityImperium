using Imperium.Economy;
using Imperium.MapObjects;
using UnityEngine;

namespace Imperium.AI
{
    [System.Serializable]
    public class AsteroidScout : MapObjectScout
    {
        public ResourceType resourceType;
        public int resourceQuantity;

        public AsteroidScout(long mapObjectId, float time, Vector3 position, ResourceType resourceType, int resourceQuantity): base(mapObjectId, position, time)
        {
            this.resourceType = resourceType;
            this.resourceQuantity = resourceQuantity;
        }
    }
}