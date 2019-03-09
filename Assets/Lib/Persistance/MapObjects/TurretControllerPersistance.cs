using Imperium.MapObjects;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class TurretControllerPersistance
    {
        public long firePriorityID;
        public bool isFiring;
        public MapObjectPersitance mapObjectPersitance;
        public Turret turret;
        public TurretType turretType;

        public TurretControllerPersistance(long firePriorityID, bool isFiring, MapObjectPersitance mapObjectPersitance, Turret turret, TurretType turretType)
        {
            this.firePriorityID = firePriorityID;
            this.isFiring = isFiring;
            this.mapObjectPersitance = mapObjectPersitance;
            this.turret = turret;
            this.turretType = turretType;
        }
    }
}