using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Persistence;
using Imperium.Enum;
public class GameInitializer : MonoBehaviour
{


    public static GameInitializer Instance { get; private set; }
    public GameSceneData gameSceneData;

    [SerializeField]
    private GameObject playerManagerPrefab;
    [SerializeField]
    private GameObject selectionPanelPrefab;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        try
        {
            gameSceneData = SceneManager.Instance.CurrentGameSceneData;
        }
        catch
        {
            gameSceneData = GameSceneData.NewGameDefault();
        }

        PlayerDatabase.Instance.SetUpDatabase(gameSceneData.players.Count);

        foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipPersistence shipPersistence in playerPersistance.Ships)
            {
                Spawner.Instance.SpawnShip(shipPersistence.shipType, playerPersistance.PlayerNumber, shipPersistence.position, Quaternion.identity);
            }
        }

        GameObject playerManager = CreatePlayerManager();
        playerManager.SetActive(true);
    }


    void Update()
    {

    }


    private GameObject CreatePlayerManager()
    {
        int player = -1;
        for (int j = 0; j < GameInitializer.Instance.gameSceneData.players.Count; j++)
        {
            if (GameInitializer.Instance.gameSceneData.players[j].playerType == PlayerType.Real)
            {
                player = GameInitializer.Instance.gameSceneData.players[j].PlayerNumber;
            }

        }

        if(player == -1)
        {
            return null;
        }
        else
        {
            GameObject playerManager = Instantiate(playerManagerPrefab);
            GameObject selectionPanel = Instantiate(this.selectionPanelPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
            GameObject constructionSection = selectionPanel.GetComponentInChildren<ConstructionSection>().gameObject;

            MouseCommandsController mouseCommandsController = playerManager.GetComponent<MouseCommandsController>();
            mouseCommandsController.selectPanel = selectionPanel;
            mouseCommandsController.constructionSection = constructionSection;


            return playerManager;
        }
        
        
    }
}
