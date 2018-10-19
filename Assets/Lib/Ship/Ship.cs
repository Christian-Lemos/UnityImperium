using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Combat.Turret;

namespace Imperium
{
    public class Ship
    {
        public string Name { get; set; }
        public Stats stats;

        public Ship(string name, Stats stats)
        {
            Name = name;    
            this.stats = stats;
        }
    }
}

