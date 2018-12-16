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
            switch (type)
            {
                case (ShipType.Test):
                    stats = new Stats(200, 200, 10, 10f);
                    shipName = "Test Ship";
                    break;
                case (ShipType.MotherShip):
                    stats = new Stats(2000, 1500, 15, 15f);
                    shipName = "MotherShip";
                    break;
                default:
                    throw new System.Exception("Type of ship not supported");
            }

            return new Ship(shipName, stats);
        }

    }
}

