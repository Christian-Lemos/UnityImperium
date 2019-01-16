using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
public class Spawner : MonoBehaviour {
    [System.Serializable]
    public struct Assosiation
    {
        public ShipType shipType;
        public GameObject prefab;
    }
    public Assosiation[] assosiations;

    private Dictionary<ShipType, GameObject> true_associations = new Dictionary<ShipType, GameObject>();
    private PlayerDatabase playerDatabase;

    public static Spawner Instance;

    private void Awake()
    {
        playerDatabase = GetComponent<PlayerDatabase>();
        for(int i = 0; i < assosiations.Length; i++)
        {
            true_associations.Add(assosiations[i].shipType, assosiations[i].prefab);
        }
        Instance = this;
    }


    public GameObject SpawnShip(ShipType type, int player, Vector3 position, Quaternion rotation)
    {
        if (!playerDatabase.IsValidPlayer(player))
        {
            throw new System.Exception("Not a valid player");
        }

        GameObject prefab = true_associations[type];
        if(prefab == null)
        {
            throw new System.Exception("Ship type not supported");
        }
        else
        {
            GameObject newShip = Instantiate(prefab, position, rotation);
            playerDatabase.AddToPlayer(newShip, player);
            return newShip;
        }
    }
}
