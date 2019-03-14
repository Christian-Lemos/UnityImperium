using Imperium;
using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour, ISerializable<List<PlayerPersistance>>
{
    public List<HashSet<GameObject>> playerObjects = new List<HashSet<GameObject>>();
    private List<Dictionary<ResourceType, int>> playerResources = new List<Dictionary<ResourceType, int>>();
    public static PlayerDatabase Instance { get; private set; }

    public void AddResourcesToPlayer(ResourceType resourceType, int total, int player)
    {
        if (IsValidPlayer(player))
        {
            playerResources[player][resourceType] += total;
            //Debug.Log(resourceType.ToString() + ", " + player + ", " + playerResources[player][resourceType]);
        }
    }

    public void AddToPlayer(GameObject target, int player)
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
        int obj_a_player = GetObjectPlayer(obj_a);
        int obj_b_player = GetObjectPlayer(obj_b);
        return obj_a_player == obj_b_player;
    }

    public int GetObjectPlayer(GameObject obj)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].Contains(obj))
            {
                return i;
            }
        }

        return -1;
    }

    public Dictionary<ResourceType, int> GetPlayerResources(int player)
    {
        return playerResources[player];
    }

    public bool IsAtDatabase(GameObject obj)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].Contains(obj))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFromPlayer(GameObject obj, int player)
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

    public bool IsValidPlayer(int player)
    {
        try
        {
            HashSet<GameObject> playerSet = playerObjects[player];
            return playerSet != null;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerator PassiveResoursesAdderIEnumerator()
    {
        while (true)
        {
            for (int i = 0; i < playerObjects.Count; i++)
            {
                StartCoroutine(AddResourcesToPlayerIEnumerator(i));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void RemoveFromPlayer(GameObject target, int player)
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

    public HashSet<GameObject> GetObjects(int player)
    {
        if (IsValidPlayer(player))
        {
            return playerObjects[player];
        }
        return null;
    }

    public void SetUpDatabase(int playerCount)
    {
        //playerObjects = new List<GameObject>[playerCount];
        //playerResources = new Dictionary<ResourceType, int>[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            playerObjects.Add(new HashSet<GameObject>());
            playerResources.Add(new Dictionary<ResourceType, int>());

            foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
            {
                playerResources[i].Add(resource, 0);
            }
        }
        StartCoroutine(PassiveResoursesAdderIEnumerator());
    }

    private IEnumerator AddResourcesToPlayerIEnumerator(int player)
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
            yield return null;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public List<PlayerPersistance> Serialize()
    {
        List<PlayerPersistance> playerPersistances = new List<PlayerPersistance>();
        for (int i = 0; i < playerResources.Count; i++)
        {
            PlayerType playerType = SceneManager.Instance.currentGameSceneData.players[i].playerType;
            List<ShipControllerPersistance> shipControllerPersistances = new List<ShipControllerPersistance>();
            List<ResourcePersistance> resourcePersistances = new List<ResourcePersistance>();
            List<StationControllerPersistance> stationControllerPersistances = new List<StationControllerPersistance>();
            foreach (GameObject @gameObject in playerObjects[i])
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

            foreach (KeyValuePair<ResourceType, int> keyValuePair in playerResources[i])
            {
                resourcePersistances.Add(new ResourcePersistance(keyValuePair.Key, keyValuePair.Value));
            }

            playerPersistances.Add(new PlayerPersistance(SceneManager.Instance.currentGameSceneData.players[i].playerNumber, playerType, resourcePersistances, shipControllerPersistances, stationControllerPersistances));
        }
        return playerPersistances;
    }

    public ISerializable<List<PlayerPersistance>> SetObject(List<PlayerPersistance> serializedObject)
    {
        throw new System.NotImplementedException();
    }
}