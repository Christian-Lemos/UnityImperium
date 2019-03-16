using Imperium.MapObjects;
using Imperium.Persistence.ShipModules;
using System.Collections.Generic;

namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class ShipControllerPersistance
    {
        public FleetCommandQueuePersistance fleetCommandQueuePersistance;
        public MapObjectPersitance mapObjectPersitance;
        public Ship ship;
        public ShipType shipType;
        public bool initialized;

        #region ShipModules

        public MineControllerPersistance mineControllerPersistance;
        public ResourceStoragePersistance resourceStoragePersistance;

        #endregion ShipModules

        public List<TurretControllerPersistance> turretControllerPersistances = new List<TurretControllerPersistance>();

        public ShipControllerPersistance(Ship ship, ShipType shipType, MapObjectPersitance mapObjectPersitance, FleetCommandQueuePersistance fleetCommandQueuePersistance, List<TurretControllerPersistance> turretControllerPersistances, bool initialized)
        {
            this.ship = ship;
            this.shipType = shipType;
            this.mapObjectPersitance = mapObjectPersitance;
            this.fleetCommandQueuePersistance = fleetCommandQueuePersistance;
            this.turretControllerPersistances = turretControllerPersistances;
            this.initialized = initialized;
        }

        public ShipControllerPersistance(Ship ship, ShipType shipType, MapObjectPersitance mapObjectPersitance, FleetCommandQueuePersistance fleetCommandQueuePersistance, bool initialized)
        {
            this.ship = ship;
            this.shipType = shipType;
            this.mapObjectPersitance = mapObjectPersitance;
            this.fleetCommandQueuePersistance = fleetCommandQueuePersistance;
            this.initialized = initialized;
        }
    }
}