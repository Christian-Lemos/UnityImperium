using Imperium.Enum;
using System.Collections.Generic;

namespace Imperium.Economy
{
    [System.Serializable]
    public class ShipConstruction
    {
        public ShipType shipType;

        public int constructionTime;
        public List<ResourceCost> resourceCosts;

        public ShipConstruction(ShipType shipType, int constructionTime, List<ResourceCost> resourceCosts)
        {
            this.shipType = shipType;
            this.constructionTime = constructionTime;
            this.resourceCosts = resourceCosts;
        }
    }
}