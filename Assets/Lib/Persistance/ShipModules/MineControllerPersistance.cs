using Imperium.Misc;

namespace Imperium.Persistence.ShipModules
{
    [System.Serializable]
    public class MineControllerPersistance
    {
        public bool isMining;
        public int miningExtractionQuantity;
        public float miningInterval;
        public Timer miningTimer;

        public MineControllerPersistance()
        {
        }

        public MineControllerPersistance(bool isMining, int miningExtractionQuantity, float miningInterval, Timer miningTimer)
        {
            this.isMining = isMining;
            this.miningExtractionQuantity = miningExtractionQuantity;
            this.miningInterval = miningInterval;
            this.miningTimer = miningTimer;
        }
    }
}