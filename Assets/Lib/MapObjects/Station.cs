using Imperium.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium.MapObjects
{
    [System.Serializable]
    public class Station
    {
        public string iconName;
        public string name;

        public Texture stationIcon;

        public CombatStats combatStats;
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Station(string name, CombatStats combatStats, string iconName)
        {
            this.name = name;
            this.combatStats = combatStats;
            this.iconName = iconName;
            SetTexture(iconName);
        }

        public Texture SetTexture(string iconName)
        {
            if (textures.ContainsKey(iconName))
            {
                stationIcon = textures[iconName];
            }
            else
            {
                stationIcon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + iconName) as Texture;
                textures.Add(iconName, stationIcon);
            }
            return stationIcon;
        }
    }
}