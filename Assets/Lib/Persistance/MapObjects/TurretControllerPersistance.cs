using Imperium.MapObjects;
using Imperium.Misc;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class TurretControllerPersistance
    {
        public long targetID;
        public long firePriorityID;
        public bool isReloading;
        public int turretIndex;
        public Timer timer;
        public Turret turret;
        public TurretType turretType;

        public TurretControllerPersistance(long targetID, long firePriorityID, bool isReloading, int turretIndex, Timer timer, Turret turret, TurretType turretType)
        {
            this.targetID = targetID;
            this.firePriorityID = firePriorityID;
            this.isReloading = isReloading;
            this.turretIndex = turretIndex;
            this.timer = timer;
            this.turret = turret;
            this.turretType = turretType;
        }
    }
}