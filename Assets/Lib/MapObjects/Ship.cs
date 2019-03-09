using Imperium.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Imperium
{
    [System.Serializable]
    public class Ship
    {
        public float angularSpeed;
        public string iconName;
        public string name;
        public Texture shipIcon;
        public CombatStats combatStats;
        public float speed;
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Ship(string name, CombatStats combatStats, string iconName, float speed, float angularSpeed)
        {
            this.name = name;
            this.combatStats = combatStats;

            this.iconName = iconName;
            this.speed = speed;
            this.angularSpeed = angularSpeed;
            SetTexture(iconName);
        }

        public Texture SetTexture(string iconName)
        {
            if (textures.ContainsKey(iconName))
            {
                shipIcon = textures[iconName];
            }
            else
            {
                shipIcon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + iconName) as Texture;
                textures.Add(iconName, shipIcon);
            }
            return shipIcon;
        }
    }
}