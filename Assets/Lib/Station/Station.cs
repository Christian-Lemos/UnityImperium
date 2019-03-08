using System.Collections.Generic;
using UnityEngine;

namespace Imperium
{
    public class Station
    {
        public string iconName;
        public string name;

        public Texture stationIcon;

        public Stats stats;
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Station(string name, Stats stats, string iconName)
        {
            this.name = name;
            this.stats = stats;
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