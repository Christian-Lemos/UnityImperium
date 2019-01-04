using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
namespace Imperium.Persistence
{
    [System.Serializable]
    public class ResourcePersistance
    {
        public ResourceType ResourceType;
        public int Quantity;

        public ResourcePersistance(ResourceType resourceType, int quantity)
        {
            ResourceType = resourceType;
            Quantity = quantity;
        }
    }
}

