using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Imperium.Economy;
namespace Imperium.Research
{
    [System.Serializable]
    public class Research
    {
        public string name;
        public string description;
        public Texture texture;
        public ResearchType reserachType;
        public List<ResourceQuantity> resourceQuantities; 
        public bool disabled;
        public int duration;
        public Research(string name, string description, string imageName, ResearchType reserachType, bool disabled, int duration)
        {
            this.name = name;
            this.description = description;
            this.reserachType = reserachType;
            this.disabled = disabled;
            this.duration = duration;
            LoadImage(imageName);
        }

        public Texture LoadImage(string imageName)
        {
            if(imageDictonary == null)
            {
                 imageDictonary = new Dictionary<string, Texture>();
            }

            if (imageDictonary.ContainsKey(imageName))
            {
                texture = imageDictonary[imageName];
            }
            else
            {
                texture = Resources.Load(imageName) as Texture;
                imageDictonary.Add(imageName, texture);
            }
            return texture;
        }

        private static Dictionary<string, Texture> imageDictonary;
    }
}