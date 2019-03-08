using UnityEngine;
using UnityEditor;
using Imperium;
namespace Imperium.Persistence
{
    [System.Serializable]
    public class ShipControllerPersistance 
    {
        public float lowestTurretRange;
        public Stats stats;

        public ShipControllerPersistance(float lowestTurretRange, Stats stats)
        {
            this.lowestTurretRange = lowestTurretRange;
            this.stats = stats;
        }
    }
}
