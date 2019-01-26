using Imperium.Enum;
using System.Collections.Generic;

namespace Imperium.Persistence
{
    [System.Serializable]
    public class PlayerPersistance
    {
        public int PlayerNumber;
        public PlayerType playerType;
        public List<ResourcePersistance> Resources;
        public List<ShipPersistence> Ships;

        public PlayerPersistance(PlayerType playerType, int playerNumber, List<ShipPersistence> ships, List<ResourcePersistance> resources)
        {
            this.playerType = playerType;
            PlayerNumber = playerNumber;
            Ships = ships;
            Resources = resources;
        }
    };
}