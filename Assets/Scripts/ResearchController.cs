using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Imperium.Research;
using Imperium.Misc;

public class ResearchController : MonoBehaviour
{
    public List<ResearchCategory> researchCategories;
    public List<ResearchTree> researchTrees;

    [System.Serializable]
    public class OnGoingResearch
    {
        public Timer timer;
        public ResearchNode researchNode;

        public OnGoingResearch(Timer timer, ResearchNode researchNode)
        {
            this.timer = timer;
            this.researchNode = researchNode;
        }
    }

    private int player;

    public List<OnGoingResearch> onGoingResearches = new List<OnGoingResearch>();

    // Use this for initialization
    void Start()
    {
        int player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);
        List<ResearchTree> allResearchTrees = PlayerDatabase.Instance.GetResearchTrees(player);

        for(int i = 0; i < researchCategories.Count; i++)
        {
            for(int j = 0; j < allResearchTrees.Count; j++)
            {
                if(allResearchTrees[j].researchCategory == researchCategories[i])
                {
                    researchTrees.Add(allResearchTrees[j]);
                    break;
                }
            }
        }

        player = PlayerDatabase.Instance.GetObjectPlayer(this.gameObject);

    }

    private void Update()
    {
        if(onGoingResearches.Count > 0)
        {
            onGoingResearches[0].timer.Execute();
        }
    }

    public bool DoResearch(ResearchNode researchNode)
    {
        if(ResearchManager.Instance.GetOnGoingResearchNode(this.gameObject, player) == null)
        {
            ResearchManager.Instance.ScheduleResearch(researchNode, this.gameObject, player);
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
}
