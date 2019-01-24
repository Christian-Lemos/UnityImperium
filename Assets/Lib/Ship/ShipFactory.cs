using Imperium.Misc;
using Imperium.Enum;
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
            string shipIcon;
            switch (type)
            {
                case (ShipType.Test):
                    stats = new Stats(200, 200, 10, 40f);
                    shipName = "Test Ship";
                    shipIcon = "ship_icon";
                    break;
                case (ShipType.MotherShip):
                    stats = new Stats(2000, 1500, 15, 45f);
                    shipName = "MotherShip";
                    shipIcon = "ship_icon";
                    break;
                case (ShipType.ConstructionShip):
                    stats = new Stats(100, 100, 5, 45f);
                    shipName = "Construction Ship";
                    shipIcon = "construction_ship_icon";
                    break;
                default:
                    throw new System.Exception("Type of ship not supported");
            }

            return new Ship(shipName, stats, shipIcon);
        }

    }
}

