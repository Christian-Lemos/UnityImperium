using Imperium;
using Imperium.Misc;
using Imperium.Research;
using System.Collections.Generic;
using UnityEngine;

public class ResearchController : MonoBehaviour
{
    public List<OnGoingResearch> onGoingResearches = new List<OnGoingResearch>();
    public List<ResearchCategory> researchCategories;
    public List<ResearchTree> researchTrees;

    private Player player;

    public bool DoResearch(ResearchNode researchNode)
    {
        if (ResearchManager.Instance.GetOnGoingResearchNode(gameObject, player) == null)
        {
            ResearchManager.Instance.ScheduleResearch(researchNode, gameObject, player);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FinishResearch()
    {
        onGoingResearches[0].researchNode.completed = true;
        onGoingResearches.RemoveAt(0);
    }

    // Use this for initialization
    private void Start()
    {
        player = PlayerDatabase.Instance.GetObjectPlayer(gameObject);

        List<ResearchTree> allResearchTrees = PlayerDatabase.Instance.GetResearchTrees(player);

        for (int i = 0; i < researchCategories.Count; i++)
        {
            for (int j = 0; j < allResearchTrees.Count; j++)
            {
                if (allResearchTrees[j].researchCategory == researchCategories[i])
                {
                    researchTrees.Add(allResearchTrees[j]);
                    break;
                }
            }
        }
        
    }

    private void Update()
    {
        if (onGoingResearches.Count > 0)
        {
            onGoingResearches[0].timer.Execute();
        }
    }

    [System.Serializable]
    public class OnGoingResearch
    {
        public ResearchNode researchNode;
        public Timer timer;

        public OnGoingResearch(Timer timer, ResearchNode researchNode)
        {
            this.timer = timer;
            this.researchNode = researchNode;
        }
    }
}