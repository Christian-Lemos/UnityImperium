using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium;
using Imperium.Misc;
using Imperium.Enum;
using Imperium.Combat.Turret;
namespace Imperium
{
    public class ShipFactory : Singleton<ShipFactory>
    {


        private ShipFactory()
        {

        }

        public Ship CreateShip(ShipType type)
        {
            Stats stats;
            string shipName;
            switch (type)
            {
                case (ShipType.Test):
                    stats = new Stats(200, 200, 10, 10f);
                    shipName = "Test Ship";
                    break;
                default:
                    throw new System.Exception("Type of ship not supported");
            }

            return new Ship(shipName, stats);
        }

    }
}

