using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Imperium
{
    public class Station
    {

        public Station(string name, Stats stats, string iconName)
        {
            Name = name;
            StationStats = stats; 
            StationIcon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + iconName) as Texture;
        }

        public string Name { get; private set; }
        public Texture StationIcon { get; private set; }
        public Stats StationStats { get; private set; }
    }
}

