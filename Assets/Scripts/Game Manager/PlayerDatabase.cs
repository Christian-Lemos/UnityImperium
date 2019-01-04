using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Persistence;
using Imperium.Enum;
[RequireComponent(typeof(Spawner))]
public class PlayerDatabase : MonoBehaviour {

    public int playerCount;
    
    public List<List<GameObject>> playerObjects = new List<List<GameObject>>();

    private List<Dictionary<ResourceType, int>> playerResources = new List<Dictionary<ResourceType, int>>();
    public static PlayerDatabase INSTANCE { get; private set; }
    public GameSceneData gameSceneData;
    private Spawner spawner;
    void Awake ()
    {
        INSTANCE = this;
        try
        {
            gameSceneData = SceneManager.Instance.CurrentGameSceneData;
        }
        catch
        {
            gameSceneData = GameSceneData.NewGameDefault();
        }

        this.playerCount = gameSceneData.players.Count;


        //playerObjects = new List<GameObject>[playerCount];
        //playerResources = new Dictionary<ResourceType, int>[playerCount];
        
        for (int i = 0; i < playerCount; i++)
        {
            playerObjects.Add (new List<GameObject>());
            playerResources.Add(new Dictionary<ResourceType, int>());

            foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
            {
                playerResources[i].Add(resource, 0);
            }
        }
    }

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipPersistence shipPersistence in playerPersistance.Ships)
            {
                spawner.SpawnShip(shipPersistence.shipType, playerPersistance.PlayerNumber, shipPersistence.position, Quaternion.identity);
            }
        }

        StartCoroutine(PassiveResoursesAdderIEnumerator());
    }
   

    public void AddResourcesToPlayer(ResourceType resourceType, int total, int player)
    {
        if(IsValidPlayer(player))
        {
            playerResources[player][resourceType] += total;
            //Debug.Log(resourceType.ToString() + ", " + player + ", " + playerResources[player][resourceType]);
        }
    }

    public Dictionary<ResourceType, int> GetPlayerResources(int player)
    {
        return playerResources[player];
    }

    public void AddToPlayer(GameObject target, int player)
    {
        if(IsValidPlayer(player))
        {
            List<GameObject> playerList = playerObjects[player];
            if (playerList.Find(x => x.Equals(target)) == null)
            {
                playerList.Add(target);
            }
        }
    }

    public bool IsValidPlayer(int player)
    {
        try
        {
            List<GameObject> playerList = playerObjects[player];
            return playerList != null;
        }
        catch
        {
            return false;
        }
    }

    public void RemoveFromPlayer(GameObject target, int player)
    {
        List<GameObject> playerList = playerObjects[player];

        if (playerList == null)
        {
            throw new System.Exception("Player not found");
        }
        else if (playerList.Find(x => x.Equals(target)))
        {
            playerList.Remove(target);
        }
    }

    public bool IsFromPlayer(GameObject obj, int player)
    {
        if(IsValidPlayer(player))
        {
            List<GameObject> playerList = playerObjects[player];
            GameObject encontrado = playerList.Find(x => x.Equals(obj));
            return encontrado != null;
        }
        else
        {
            throw new System.Exception("Player not found");
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
        for(int i = 0; i < playerObjects.Count; i++)
        {
            GameObject found = playerObjects[i].Find(x => x.Equals(obj));
            if(found != null)
            {
                return i;
            }
        }

        return -1;
    }
    
    public IEnumerator PassiveResoursesAdderIEnumerator()
    {
        while(true)
        {
            
            for(int i = 0; i < playerObjects.Count; i++)
            {
                StartCoroutine(AddResourcesToPlayerIEnumerator(i));
            }
            yield return new WaitForSeconds(1f);
        }
    }

  

    private IEnumerator AddResourcesToPlayerIEnumerator(int player)
    {
        for (int i = 0; i < playerObjects[player].Count; i++)
        {
            PassiveResourceAdder adder = playerObjects[player][i].GetComponent<PassiveResourceAdder>();
            if (adder != null)
            {
                foreach(KeyValuePair<ResourceType, int> entry in adder.true_associations)
                {
                    AddResourcesToPlayer(entry.Key, entry.Value, player);
                }
            }
            yield return null;
        }
    }

}
