using Imperium.Enum;
using Imperium.Misc;

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
            float speed;
            float angularSpeed;
            switch (type)
            {
                case (ShipType.Test):
                    stats = new Stats(200, 200, 10, 40f);
                    shipName = "Test Ship";
                    shipIcon = "ship_icon";
                    speed = 2f;
                    angularSpeed = 50f;
                    break;

                case (ShipType.MotherShip):
                    stats = new Stats(2000, 1500, 15, 45f);
                    shipName = "MotherShip";
                    shipIcon = "ship_icon";
                    speed = 0.4f;
                    angularSpeed = 17f;
                    break;

                case (ShipType.ConstructionShip):
                    stats = new Stats(100, 100, 5, 45f);
                    shipName = "Construction Ship";
                    shipIcon = "construction_ship_icon";
                    speed = 0.95f;
                    angularSpeed = 45f;
                    break;

                case (ShipType.Freighter):
                    stats = new Stats(100, 100, 5, 45f);
                    shipName = "Freighter";
                    shipIcon = "freighter_icon";
                    speed = 1f;
                    angularSpeed = 25f;
                    break;

                default:
                    throw new System.Exception("Type of ship not supported");
            }

            return new Ship(shipName, stats, shipIcon, speed, angularSpeed);
        }
    }
}