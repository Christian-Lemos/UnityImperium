using Imperium.Enum;
using System.Collections.Generic;

namespace Imperium.Economy
{
    [System.Serializable]
    public class StationConstruction
    {
        public List<ResourceCost> resourceCosts;
        public StationType stationType;

        public StationConstruction(StationType stationType, List<ResourceCost> resourceCosts)
        {
            this.stationType = stationType;
            this.resourceCosts = resourceCosts;
        }
    }
}