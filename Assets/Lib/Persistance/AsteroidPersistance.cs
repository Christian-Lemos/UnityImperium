using UnityEngine;
using Imperium.Enum;
using System.Collections;

namespace Imperium.Persistence
{
    public class AsteroidPersistance
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public ResourceType resourceType;
        public int resourceQuantity;

        public AsteroidPersistance(Vector3 position, Quaternion rotation, Vector3 scale, ResourceType resourceType, int resourceQuantity)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.resourceType = resourceType;
            this.resourceQuantity = resourceQuantity;
        }
    }
}

