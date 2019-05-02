using Imperium.Persistence.MapObjects;
using System.Collections.Generic;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class PlayerPersistance
    {
        public Player player;
        public List<ResourcePersistance> resources;
        public List<ShipControllerPersistance> ships;
        public List<StationControllerPersistance> stations;

        public PlayerPersistance(Player player, List<ResourcePersistance> resources, List<ShipControllerPersistance> ships, List<StationControllerPersistance> stations)
        {
            this.player = player;
            this.resources = resources;
            this.ships = ships;
            this.stations = stations;
        }
    };
}