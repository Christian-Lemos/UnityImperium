using UnityEngine;
using UnityEditor;
using Imperium;
using Imperium.MapObjects;
using Imperium.Navigation;
using System.Collections.Generic;
namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class ShipControllerPersistance
    {
        public Ship ship;
        public ShipType shipType;
        public MapObjectPersitance mapObjectPersitance;
        public FleetCommandQueuePersistance fleetCommandQueuePersistance;
        public List<TurretControllerPersistance> turretControllerPersistances;

        public ShipControllerPersistance(Ship ship, ShipType shipType, MapObjectPersitance mapObjectPersitance, FleetCommandQueuePersistance fleetCommandQueuePersistance, List<TurretControllerPersistance> turretControllerPersistances)
        {
            this.ship = ship;
            this.shipType = shipType;
            this.mapObjectPersitance = mapObjectPersitance;
            this.fleetCommandQueuePersistance = fleetCommandQueuePersistance;
            this.turretControllerPersistances = turretControllerPersistances;
        }

        public ShipControllerPersistance(Ship ship, ShipType shipType, MapObjectPersitance mapObjectPersitance, FleetCommandQueuePersistance fleetCommandQueuePersistance)
        {
            this.ship = ship;
            this.shipType = shipType;
            this.mapObjectPersitance = mapObjectPersitance;
            this.fleetCommandQueuePersistance = fleetCommandQueuePersistance;
        }
    }
}
