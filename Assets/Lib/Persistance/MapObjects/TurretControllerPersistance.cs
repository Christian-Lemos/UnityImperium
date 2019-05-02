using Imperium.Combat;
using Imperium.MapObjects;
using Imperium.Misc;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class TurretControllerPersistance
    {
        public long targetID;
        public long firePriorityID;
        public FireStage fireStage;
        public int turretIndex;
        public Timer reloadTimer;
        public Turret turret;
        public TurretType turretType;
        public int salvoCount;
        public Timer salvoTimer;

        public TurretControllerPersistance(long targetID, long firePriorityID, FireStage fireStage, int turretIndex, Timer reloadTimer, Turret turret, TurretType turretType, int salvoCount, Timer salvoTimer)
        {
            this.targetID = targetID;
            this.firePriorityID = firePriorityID;
            this.fireStage = fireStage;
            this.turretIndex = turretIndex;
            this.reloadTimer = reloadTimer;
            this.turret = turret;
            this.turretType = turretType;
            this.salvoCount = salvoCount;
            this.salvoTimer = salvoTimer;
        }
    }
}