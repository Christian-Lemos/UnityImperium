using Imperium.Persistence.MapObjects;
using System.Collections.Generic;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class PlayerPersistance
    {
        public int playerNumber;
        public PlayerType playerType;
        public List<ResourcePersistance> resources;
        public List<ShipControllerPersistance> ships;
        public List<StationControllerPersistance> stations;

        public PlayerPersistance(int playerNumber, PlayerType playerType, List<ResourcePersistance> resources, List<ShipControllerPersistance> ships, List<StationControllerPersistance> stations)
        {
            this.playerNumber = playerNumber;
            this.playerType = playerType;
            this.resources = resources;
            this.ships = ships;
            this.stations = stations;
        }
    };
}