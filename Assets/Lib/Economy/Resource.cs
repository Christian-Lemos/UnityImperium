using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;

namespace Imperium.Economy
{
    public class Resource
    {
        public string Name { get; private set; }
        public Texture Icon { get; private set; }
        public ResourceType Type { get; private set; }

        public Resource(ResourceType type)
        {
            Type = type;

            switch(type)
            {
                case ResourceType.Metal:
                    Name = "Metal";
                    Icon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + "metal_icon") as Texture;
                    break;
                case ResourceType.Crystal:
                    Name = "Crystal";
                    Icon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + "crystal_icon") as Texture;
                    break;
                case ResourceType.Energy:
                    Name = "Energy";
                    Icon = Resources.Load("icons" + System.IO.Path.DirectorySeparatorChar + "energy_icon") as Texture;
                    break;
            }
            
        }
    }
}

