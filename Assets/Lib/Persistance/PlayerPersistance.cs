using Imperium.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Imperium.Persistence
{
    [System.Serializable]
    public class PlayerPersistance
    {
        public PlayerType playerType;
        public int PlayerNumber;
        public List<ShipPersistence> Ships;
        public List<ResourcePersistance> Resources;

        public PlayerPersistance(PlayerType playerType, int playerNumber, List<ShipPersistence> ships, List<ResourcePersistance> resources)
        {
            this.playerType = playerType;
            PlayerNumber = playerNumber;
            Ships = ships;
            Resources = resources;
        }
    };
}
