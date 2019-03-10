using System.Collections.Generic;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class ShipConstructionManagerPersistance
    {
        public List<Construction> constructions;

        public ShipConstructionManagerPersistance(List<Construction> constructions)
        {
            this.constructions = constructions;
        }

        [System.Serializable]
        public class Construction
        {
            public ShipConstructionManager.OnGoingShipConstruction onGoingShipConstruction;
            public long sourceID;

            public Construction(long sourceID, ShipConstructionManager.OnGoingShipConstruction onGoingShipConstruction)
            {
                this.sourceID = sourceID;
                this.onGoingShipConstruction = onGoingShipConstruction;
            }
        }
    }
}