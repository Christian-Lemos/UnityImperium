using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
namespace Imperium.Economy
{
    [System.Serializable]
    public class Construction<T>
    {
        public T ConstructionType;

        public int ConstructionTime;
        public List<ResourceCost> ResourceCosts;

        public Construction(T constructionType, int constructionTime, List<ResourceCost> resourceCosts)
        {
            ConstructionType = constructionType;
            ConstructionTime = constructionTime;
            ResourceCosts = resourceCosts;
        }
    }

}

