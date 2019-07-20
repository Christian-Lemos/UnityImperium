using Imperium;
using Imperium.Misc;
using Imperium.Persistence;
using Imperium.Research;
using System;
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

    private Dictionary<Player, HashSet<ResearchBehaviour>> playerResearchBehaviours = new Dictionary<Player, HashSet<ResearchBehaviour>>();

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
        foreach (Player player in PlayerDatabase.Instance.players)
        {
            playerResearchBehaviours.Add(player, new HashSet<ResearchBehaviour>());
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
        List<Type> types = ResearchFactory.Instance.GetBehaviours(onGoingResearches[player][0].researchNode.research.reserachType);
        foreach(Type type in types)
        {
            ResearchBehaviour researchBehaviour = (ResearchBehaviour) this.gameObject.GetComponent(type);
            if(researchBehaviour == null)
            {
                researchBehaviour = (ResearchBehaviour) this.gameObject.AddComponent(type);
                researchBehaviour.player = player;
                this.playerResearchBehaviours[player].Add(researchBehaviour);

            }
            else if(researchBehaviour is LeveledResearchBehaviour)
            {
                ((LeveledResearchBehaviour) researchBehaviour).UpdateLevel();
            }
        }

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

