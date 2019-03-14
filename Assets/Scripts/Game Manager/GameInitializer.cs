using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameSceneData gameSceneData;

    [SerializeField]
    private GameObject playerManager;

    [SerializeField]
    private GameObject selectionPanelPrefab;

    public static GameInitializer Instance { get; private set; }

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
        try
        {
            gameSceneData = SceneManager.Instance.currentGameSceneData;
        }
        catch
        {
            gameSceneData = GameSceneData.NewGameDefault();
        }

        PlayerDatabase.Instance.SetUpDatabase(gameSceneData.players.Count);

        foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipControllerPersistance shipPersistence in playerPersistance.ships)
            {
                Spawner.Instance.SpawnShip(shipPersistence.shipType, playerPersistance.playerNumber, shipPersistence.mapObjectPersitance.localPosition, Quaternion.identity);
            }
        }

        CreateAsteroidFields();

        SetUpPlayerManager();
    }

    private void CreateAsteroidFields()
    {
        // Vector3 position = new Vector3(0, 0, 0);
        // Vector3 size = new Vector3(15, 3, 15);

        // GameObject asteroidField = Spawner.Instance.SpawnAsteroidField(AsteroidFieldAsteroidSettings.CreateDefaultSettings(), position, size, true);
        // asteroidField.GetComponent<AsteroidFieldController>().SpawnAsteroidsOnField(true);
        foreach (AsteroidFieldControllerPersistance asteroidFieldPersistance in gameSceneData.asteroidFields)
        {
            AsteroidFieldAsteroidSettings asteroidFieldAsteroidSettings = new AsteroidFieldAsteroidSettings();
            asteroidFieldAsteroidSettings.SetObject(asteroidFieldPersistance.AsteroidFieldAsteroidSettingsPersistance);
            GameObject asteroidField = Spawner.Instance.SpawnAsteroidField(asteroidFieldAsteroidSettings, asteroidFieldPersistance.mapObjectPersitance.localPosition, asteroidFieldPersistance.mapObjectPersitance.localScale, false);

            AsteroidFieldController asteroidFieldController = asteroidField.GetComponent<AsteroidFieldController>();
            asteroidFieldController.initialized = asteroidFieldPersistance.initialized;
            asteroidFieldController.size = asteroidFieldPersistance.size;
            asteroidField.SetActive(true);

            if (!asteroidFieldController.initialized)
            {
                asteroidFieldController.SpawnAsteroidsOnField(true);
            }
        }
    }

    private void SetUpPlayerManager()
    {
        int player = -1;
        for (int j = 0; j < GameInitializer.Instance.gameSceneData.players.Count; j++)
        {
            if (GameInitializer.Instance.gameSceneData.players[j].playerType == PlayerType.Real)
            {
                player = GameInitializer.Instance.gameSceneData.players[j].playerNumber;
            }
        }

        if (player == -1)
        {
            return;
        }
        else
        {
            GameObject selectionPanel = Instantiate(selectionPanelPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
            GameObject constructionSection = selectionPanel.GetComponentInChildren<ConstructionSection>().gameObject;

            MouseCommandsController mouseCommandsController = playerManager.GetComponent<MouseCommandsController>();
            mouseCommandsController.selectPanel = selectionPanel;
            mouseCommandsController.constructionSection = constructionSection;

            playerManager.SetActive(true);
        }
    }
}