using UnityEngine;
using UnityEditor;
using Imperium;
namespace Imperium.Persistence
{
    [System.Serializable]
    public class ObjectControllerPersistance 
    {
        public float lowestTurretRange;
        public Stats stats;

        public ObjectControllerPersistance(float lowestTurretRange, Stats stats)
        {
            this.lowestTurretRange = lowestTurretRange;
            this.stats = stats;
        }
    }
}
