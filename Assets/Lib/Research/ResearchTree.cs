using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Imperium.Economy;
namespace Imperium.Research
{
    [System.Serializable]
    public class ResearchTree
    {
        [SerializeField]
        private List<ResearchNode> researchNodes;
        public ResearchCategory researchCategory;
        public ResearchTree()
        {
            this.researchNodes = new List<ResearchNode>();
        }

        public ResearchTree(List<ResearchNode> reearchNodes, ResearchCategory researchCategory)
        {
            this.ResearchNodes = researchNodes;
            this.researchCategory = researchCategory;
        }

        public ResearchNode NextNode
        {
            get
            {
                for(int i = 0; i < ResearchNodes.Count; i++)
                {
                    if(!ResearchNodes[i].completed)
                    {
                        return ResearchNodes[i];
                    }
                }
                return null;
            }
        }

        public List<ResearchNode> ResearchNodes
        {
            get
            {
                return researchNodes;
            }

            set
            {
                researchNodes = value;
            }
        }
    }
}