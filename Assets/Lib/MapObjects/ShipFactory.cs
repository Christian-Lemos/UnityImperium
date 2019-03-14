using Imperium.Combat;
using Imperium.MapObjects;
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
            CombatStats stats;

            string shipName;
            string shipIcon;
            float speed;
            float angularSpeed;
            switch (type)
            {
                case (ShipType.Test):
                    stats = new CombatStats(200, 200, 10, 40f);
                    shipName = "Test Ship";
                    shipIcon = "ship_icon";
                    speed = 5f;
                    angularSpeed = 50f;
                    break;

                case (ShipType.MotherShip):
                    stats = new CombatStats(2000, 1500, 15, 45f);
                    shipName = "MotherShip";
                    shipIcon = "ship_icon";
                    speed = 0.4f;
                    angularSpeed = 17f;
                    break;

                case (ShipType.ConstructionShip):
                    stats = new CombatStats(100, 100, 5, 45f);
                    shipName = "Construction Ship";
                    shipIcon = "construction_ship_icon";
                    speed = 3f;
                    angularSpeed = 45f;
                    break;

                case (ShipType.Freighter):
                    stats = new CombatStats(100, 100, 5, 45f);
                    shipName = "Freighter";
                    shipIcon = "freighter_icon";
                    speed = 2.5f;
                    angularSpeed = 25f;
                    break;

                default:
                    throw new System.Exception("Type of ship not supported");
            }

            return new Ship(shipName, stats, shipIcon, speed, angularSpeed);
        }
    }
}