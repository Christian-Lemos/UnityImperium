using Imperium.MapObjects;
using System.Collections.Generic;

namespace Imperium.Economy
{
    [System.Serializable]
    public class ShipConstruction
    {
        public int constructionTime;
        public List<ResourceQuantity> resourceCosts;
        public ShipType shipType;

        public ShipConstruction(ShipType shipType, int constructionTime, List<ResourceQuantity> resourceCosts)
        {
            this.shipType = shipType;
            this.constructionTime = constructionTime;
            this.resourceCosts = resourceCosts;
        }
    }
}