using Imperium;
using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Misc;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using Imperium.Research;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour, ISerializable<List<PlayerPersistance>>
{
    //public List<HashSet<GameObject>> playerObjects = new List<HashSet<GameObject>>();
    //private List<Dictionary<ResourceType, int>> playerResources = new List<Dictionary<ResourceType, int>>();
    // private List<List<ResearchTree>> researchTrees = new List<List<ResearchTree>>();

    private Dictionary<Player, HashSet<GameObject>> playerObjects = new Dictionary<Player, HashSet<GameObject>>();
    private Dictionary<Player, Dictionary<ResourceType, int>> playerResources = new Dictionary<Player, Dictionary<ResourceType, int>>();
    private Dictionary<Player, List<ResearchTree>> researchTrees = new Dictionary<Player, List<ResearchTree>>(); 

    private Dictionary<Player, Timer> playersPassiveResourcesTimers = new Dictionary<Player, Timer>();

    public HashSet<Player> players = new HashSet<Player>();

    public static PlayerDatabase Instance { get; private set; }

    public Player FindPlayaerByNumber(int number)
    {
        foreach(Player player in this.players)
        {
            if(player.Number == number)
            {
                return player;
            }
        }

        return null;
    }

    public void AddResourcesToPlayer(ResourceType resourceType, int total, Player player)
    {
        if (IsValidPlayer(player))
        {
            playerResources[player][resourceType] += total;
            //Debug.Log(resourceType.ToString() + ", " + player + ", " + playerResources[player][resourceType]);
        }
    }

    public void AddObjectToPlayer(GameObject target, Player player)
    {
        if (IsValidPlayer(player))
        {
            HashSet<GameObject> playerSet = playerObjects[player];

            if (!playerSet.Contains(target))
            {
                playerSet.Add(target);
            }
        }
    }

    public bool AreFromSamePlayer(GameObject obj_a, GameObject obj_b)
    {
        Player obj_a_player = GetObjectPlayer(obj_a);
        Player obj_b_player = GetObjectPlayer(obj_b);
        return obj_a_player.Equals(obj_b_player);
    }

    public Player GetObjectPlayer(GameObject obj)
    {
        foreach(KeyValuePair<Player, HashSet<GameObject>> objects in playerObjects)
        {
            if (objects.Value.Contains(obj))
            {
                return objects.Key;
            }
        }
        return null;
    }

    public HashSet<GameObject> GetObjects(Player player)
    {
        if (IsValidPlayer(player))
        {
            return playerObjects[player];
        }
        return null;
    }

    public Dictionary<ResourceType, int> GetPlayerResources(Player player)
    {
        return playerResources[player];
    }

    public List<ResearchTree> GetResearchTrees(Player player)
    {
        if (IsValidPlayer(player))
        {
            return researchTrees[player];
        }
        return null;
    }

    public bool IsAtDatabase(GameObject obj)
    {
        foreach (KeyValuePair<Player, HashSet<GameObject>> objects in playerObjects)
        {
            if (objects.Value.Contains(obj))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFromPlayer(GameObject obj, Player player)
    {
        if (IsValidPlayer(player))
        {
            HashSet<GameObject> playerSet = playerObjects[player];
            return playerSet.Contains(obj);
        }
        else
        {
            throw new System.Exception("Player not found");
        }
    }

    public bool IsValidPlayer(Player player)
    {
        return players.Contains(player);
    }


    public void RemoveFromPlayer(GameObject target, Player player)
    {
        HashSet<GameObject> playerSet = playerObjects[player];

        if (playerSet == null)
        {
            throw new System.Exception("Player not found");
        }
        else
        {
            playerSet.Remove(target);
        }
    }

    public List<PlayerPersistance> Serialize()
    {
        List<PlayerPersistance> playerPersistances = new List<PlayerPersistance>();


        foreach(Player player in players)
        {
            List<ShipControllerPersistance> shipControllerPersistances = new List<ShipControllerPersistance>();
            List<ResourcePersistance> resourcePersistances = new List<ResourcePersistance>();
            List<StationControllerPersistance> stationControllerPersistances = new List<StationControllerPersistance>();

            foreach(KeyValuePair<Player, HashSet<GameObject>> keyValuePair in playerObjects)
            {

            }

            foreach (GameObject @gameObject in playerObjects[player])
            {
                MapObject mapObject = @gameObject.GetComponent<MapObject>();

                if (mapObject.mapObjectType == MapObjectType.Ship)
                {
                    shipControllerPersistances.Add(mapObject.gameObject.GetComponent<ShipController>().Serialize());
                }
                else if (mapObject.mapObjectType == MapObjectType.Station)
                {
                    stationControllerPersistances.Add(mapObject.gameObject.GetComponent<StationController>().Serialize());
                }
            }

            foreach (KeyValuePair<ResourceType, int> keyValuePair in playerResources[player])
            {
                resourcePersistances.Add(new ResourcePersistance(keyValuePair.Key, keyValuePair.Value));
            }

            playerPersistances.Add(new PlayerPersistance(player, resourcePersistances, shipControllerPersistances, stationControllerPersistances));
        }

        return playerPersistances;
    }

    public ISerializable<List<PlayerPersistance>> SetObject(List<PlayerPersistance> serializedObject)
    {
        foreach (PlayerPersistance pp in serializedObject)
        {
            foreach (ShipControllerPersistance scp in pp.ships)
            {
                playerObjects[pp.player].Add(MapObject.FindByID(scp.mapObjectPersitance.id).gameObject);
            }

            foreach (StationControllerPersistance scp in pp.stations)
            {
                playerObjects[pp.player].Add(MapObject.FindByID(scp.mapObjectPersitance.id).gameObject);
            }

            foreach (ResourcePersistance resourcePersistance in pp.resources)
            {
                playerResources[pp.player][resourcePersistance.resourceType] = resourcePersistance.quantity;
            }
        }
        return this;
    }

    public void SetUpDatabase(params Player[] players)
    {
        //playerObjects = new List<GameObject>[playerCount];
        //playerResources = new Dictionary<ResourceType, int>[playerCount];

        for (int i = 0; i < players.Length; i++)
        {
            this.players.Add(players[i]);
            playerObjects.Add(players[i], new HashSet<GameObject>());
            playerResources.Add(players[i], new Dictionary<ResourceType, int>());
            researchTrees.Add(players[i], new List<ResearchTree>());
            foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
            {
                playerResources[players[i]].Add(resource, 0);
            }
            foreach (ResearchTreeDefinition researchTreeDefinition in System.Enum.GetValues(typeof(ResearchTreeDefinition)))
            {
                researchTrees[players[i]].Add(ResearchFactory.Instance.CreateResearchTree(researchTreeDefinition));
            }

            playersPassiveResourcesTimers.Add(players[i], new Timer(1f, true));
        }
    }

    private void PassiveResourceAdder(Player player)
    {
        foreach (GameObject obj in playerObjects[player])
        {
            PassiveResourceAdder adder = obj.GetComponent<PassiveResourceAdder>();
            if (adder != null)
            {
                foreach (KeyValuePair<ResourceType, int> entry in adder.true_associations)
                {
                    AddResourcesToPlayer(entry.Key, entry.Value, player);
                }
            }
        }
    }

    private void Update()
    {
        foreach(KeyValuePair<Player, Timer> keyValuePair in playersPassiveResourcesTimers)
        {
            keyValuePair.Value.Execute();
            if(keyValuePair.Value.IsFinished)
            {
                PassiveResourceAdder(keyValuePair.Key);
                keyValuePair.Value.ResetTimer();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }


}