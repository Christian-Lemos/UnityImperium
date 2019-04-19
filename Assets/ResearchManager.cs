using Imperium.Misc;
using Imperium.Research;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    [System.Serializable]
    public class OnGoingResearch
    {
        public GameObject source;
        public Timer timer;
        public ResearchNode researchNode;

        public OnGoingResearch(GameObject source, Timer timer, ResearchNode researchNode)
        {
            this.source = source;
            this.timer = timer;
            this.researchNode = researchNode;
        }
    }

    private List<List<OnGoingResearch>> onGoingResearches = new List<List<OnGoingResearch>>();

    public static ResearchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int playersCount = SceneManager.Instance.currentGameSceneData.players.Count;
        for(int i = 0; i < playersCount; i++)
        {
            onGoingResearches.Add(new List<OnGoingResearch>());
        }
    }

    void Update()
    {
        for(int i = 0; i < onGoingResearches.Count; i++)
        {
            if(onGoingResearches[i].Count > 0)
            {
                onGoingResearches[i][0].timer.Execute();
            }
        }
    }

    public void ScheduleResearch(ResearchNode researchNode, GameObject source, int player)
    {
        OnGoingResearch onGoingResearch = new OnGoingResearch(source, new Timer(researchNode.research.duration, true, () => { 
            FinishResearch(player);
        }), researchNode);
        onGoingResearches[player].Add(onGoingResearch);
    }

    private void FinishResearch(int player)
    {
        Debug.Log(onGoingResearches[player][0].researchNode.research.name + " completed");
        onGoingResearches[player][0].researchNode.completed = true;
        onGoingResearches[player].RemoveAt(0);
    }

    public ResearchNode GetOnGoingResearchNode(GameObject source, int player)
    {
        for(int i = 0; i < onGoingResearches[player].Count; i++)
        {
            if(onGoingResearches[player][i].source.Equals(source))
            {
                return onGoingResearches[player][i].researchNode;
            }
        }
        return null;
    }
}
