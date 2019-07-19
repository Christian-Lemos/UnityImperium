using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Imperium.Economy;
using System;

namespace Imperium.Research
{
    public class ResearchFactory
    {
        private static ResearchFactory _instance;

        public static ResearchFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new ResearchFactory();
                }
                return _instance;
            }
        }

        private Dictionary<ResearchType, List<Type>> researchBehaviours = new Dictionary<ResearchType, List<Type>>()
        {
            {
                ResearchType.Era1, new List<Type>()
                {
                    typeof(ShipArmorRB)
                }
            },
            {
                ResearchType.Era2, new List<Type>()
                {
                    typeof(ShipArmorRB)
                }
            }
        };

        public List<Type> GetBehaviours(ResearchType researchType)
        {
            if(!researchBehaviours.ContainsKey(researchType))
            {
                researchBehaviours.Add(researchType, new List<Type>());
            }
            return researchBehaviours[researchType];
        }


        public Research CreateResearch(ResearchType researchType, bool disabled)
        {
            string name;
            string description;
            string imageName;
            int duration;
            List<ResourceQuantity> resourceQuantities;

            switch (researchType)
            {
                case ResearchType.Era1:
                    name = "Exploration Era";
                    description = "Advances to the Exploration Era";
                    imageName = "research icon";
                    resourceQuantities = new List<ResourceQuantity>()
                    {
                        new ResourceQuantity(100, ResourceType.BlackMatter)
                    };
                    duration = 5;
                    break;
                case ResearchType.Era2:
                    name = "Expansion Era";
                    description = "Advances to the Expansion Era";
                    imageName = "research icon";
                    resourceQuantities = new List<ResourceQuantity>()
                    {
                        new ResourceQuantity(200, ResourceType.BlackMatter)
                    };
                    duration = 5;
                    break;
                case ResearchType.Era3:
                    name = "Warfare era";
                    description = "Advances to the Warfare Era";
                    imageName = "research icon";
                    resourceQuantities = new List<ResourceQuantity>()
                    {
                        new ResourceQuantity(400, ResourceType.BlackMatter)
                    };
                    duration = 5;
                    break;
                case ResearchType.Era4:
                    name = "Imperial Era";
                    description = "Advances to the Imperial Era";
                    imageName = "research icon";
                    resourceQuantities = new List<ResourceQuantity>()
                    {
                        new ResourceQuantity(800, ResourceType.BlackMatter)
                    };
                    duration = 5;
                    break;
                default:
                    throw new System.Exception("ResearchType " + researchType + " not suported");
                        
            }


            return new Research(name, description,"icons/" + imageName, researchType, disabled, duration);
        }

        public ResearchTree CreateResearchTree(ResearchTreeDefinition researchTreeDefinition)
        {
            switch (researchTreeDefinition)
            {
                case ResearchTreeDefinition.Eras:
                    return new ResearchTree { 
                        ResearchNodes = {
                            new ResearchNode(this.CreateResearch(ResearchType.Era1, false), ResearchEra.ExplorationEra, false),
                            new ResearchNode(this.CreateResearch(ResearchType.Era2, false), ResearchEra.ExpansionEra, false),
                            new ResearchNode(this.CreateResearch(ResearchType.Era3, false), ResearchEra.WarfareEra, false),
                            new ResearchNode(this.CreateResearch(ResearchType.Era4, false), ResearchEra.ImperialEra, false)
                        }
                    };
                default:
                    throw new System.Exception(researchTreeDefinition + " not supported");
            }
            
        }

    }
}