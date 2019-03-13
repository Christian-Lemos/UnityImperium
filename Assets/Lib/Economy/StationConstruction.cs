using Imperium.MapObjects;
using System.Collections.Generic;

namespace Imperium.Economy
{
    [System.Serializable]
    public class StationConstruction
    {
        public List<ResourceQuantity> resourceCosts;
        public StationType stationType;

        public StationConstruction(StationType stationType, List<ResourceQuantity> resourceCosts)
        {
            this.stationType = stationType;
            this.resourceCosts = resourceCosts;
        }
    }
}