using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Combat.Turret;
using UnityEngine.UI;


namespace Imperium
{
    public class Ship
    {
        public string Name { get; set; }
        public Stats ShipStats { get; private set; }
        public Texture ShipIcon { get; private set; }

        public Ship(string name, Stats stats, string iconName)
        {
            Name = name;
            this.ShipStats = stats;
            this.ShipIcon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + iconName) as Texture;
        }
    }
}

