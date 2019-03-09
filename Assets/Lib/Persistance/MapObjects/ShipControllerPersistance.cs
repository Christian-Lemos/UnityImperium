using UnityEngine;
using UnityEditor;
using Imperium;
using Imperium.MapObjects;
using Imperium.Navigation;
namespace Imperium.Persistence.MapObjects
{
    [System.Serializable]
    public class ShipControllerPersistance
    {
        public Ship ship;
        public ShipType shipType;
        public MapObjectPersitance mapObjectPersitance;
        public FleetCommandQueuePersistance fleetCommandQueuePersistance;

        public ShipControllerPersistance(Ship ship, ShipType shipType, MapObjectPersitance mapObjectPersitance, FleetCommandQueuePersistance fleetCommandQueuePersistance)
        {
            this.ship = ship;
            this.shipType = shipType;
            this.mapObjectPersitance = mapObjectPersitance;
            this.fleetCommandQueuePersistance = fleetCommandQueuePersistance;
        }
    }
}
