using Imperium;
using Imperium.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicAI : MonoBehaviour
{

    public Player player;
    public ScoutData scoutData;

    public static Dictionary<Player, StrategicAI> playerStrategicAI;

    void Start()
    {
        if(playerStrategicAI == null)
        {
            playerStrategicAI = new Dictionary<Player, StrategicAI>();
        }

        scoutData = new ScoutData(player);
        playerStrategicAI.Add(player, this);

        Spawner.Instance.ObserveShipCreation(player, OnShipCreation);
    }

    
    void Update()
    {
        scoutData.Update();
    }

    private void OnShipCreation(GameObject ship)
    {
        Debug.Log(ship.name);
    }
}
