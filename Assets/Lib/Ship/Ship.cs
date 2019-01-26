using UnityEngine;

namespace Imperium
{
    public class Ship
    {
        public Ship(string name, Stats stats, string iconName)
        {
            Name = name;
            ShipStats = stats;
            ShipIcon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + iconName) as Texture;
        }

        public string Name { get; set; }
        public Texture ShipIcon { get; private set; }
        public Stats ShipStats { get; private set; }
    }
}