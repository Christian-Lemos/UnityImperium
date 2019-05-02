using Imperium;
using Imperium.Misc;
using Imperium.Persistence;
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

    private Dictionary<Player, List<OnGoingResearch>> onGoingResearches = new Dictionary<Player, List<OnGoingResearch>>();

    public static ResearchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        foreach(Player player in PlayerDatabase.Instance.players)
        {
            onGoingResearches.Add(player, new List<OnGoingResearch>());
        }
    }

    void Update()
    {
        foreach(KeyValuePair<Player, List<OnGoingResearch>> keyValuePair in onGoingResearches)
        {
            if (keyValuePair.Value.Count > 0)
            {
                keyValuePair.Value[0].timer.Execute();
            }
        }
    }

    public void ScheduleResearch(ResearchNode researchNode, GameObject source, Player player)
    {
        OnGoingResearch onGoingResearch = new OnGoingResearch(source, new Timer(researchNode.research.duration, true, () => { 
            FinishResearch(player);
        }), researchNode);
        onGoingResearches[player].Add(onGoingResearch);
    }

    private void FinishResearch(Player player)
    {
        Debug.Log(onGoingResearches[player][0].researchNode.research.name + " completed");
        onGoingResearches[player][0].researchNode.completed = true;
        onGoingResearches[player].RemoveAt(0);
    }

    public ResearchNode GetOnGoingResearchNode(GameObject source, Player player)
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
