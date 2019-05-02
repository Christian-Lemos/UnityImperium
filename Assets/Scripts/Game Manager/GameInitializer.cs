using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameSceneData gameSceneData;

   
    public GameObject playerManager;

    public GameObject selectionPanelPrefab;

    public GameObject AIStorage;

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
        Player[] players = new Player[gameSceneData.players.Count];

        for(int i =0; i < gameSceneData.players.Count; i++)
        {
            players[i] = gameSceneData.players[i].player;
        }

        PlayerDatabase.Instance.SetUpDatabase(players);

        /*foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipControllerPersistance shipPersistence in playerPersistance.ships)
            {
                Spawner.Instance.SpawnShip(shipPersistence.shipType, playerPersistance.playerNumber, shipPersistence.mapObjectPersitance.localPosition, Quaternion.identity);
            }
        }

        CreateAsteroidFields();

        SetUpPlayerManager();*/
        SetUpPlayerManager();
        SetUpAI();
        LoadGameSceneData();
    }

    private void SetUpAI()
    {
        for (int i = 0; i < gameSceneData.players.Count; i++)
        {
            Player player = gameSceneData.players[i].player;
            if(player.PlayerType == PlayerType.AI)
            {
                StrategicAI strategicAI = AIStorage.AddComponent<StrategicAI>();
                strategicAI.player = player;
            }
        }
    }

    

    private void LoadGameSceneData()
    {

        List<GameObjectSerializedObjectAssociation<AsteroidControllerPersistance>> asteroids = new List<GameObjectSerializedObjectAssociation<AsteroidControllerPersistance>>();

        List<GameObjectSerializedObjectAssociation<AsteroidFieldControllerPersistance>> asteroidFields = new List<GameObjectSerializedObjectAssociation<AsteroidFieldControllerPersistance>>();
        List<GameObjectSerializedObjectAssociation<ShipControllerPersistance>> ships = new List<GameObjectSerializedObjectAssociation<ShipControllerPersistance>>();
        List<GameObjectSerializedObjectAssociation<BulletControllerPersistance>> bullets = new List<GameObjectSerializedObjectAssociation<BulletControllerPersistance>>();
        List<GameObjectSerializedObjectAssociation<StationControllerPersistance>> stations = new List<GameObjectSerializedObjectAssociation<StationControllerPersistance>>();

        List<MapObjectSerializedObjectAssociation> mapObjects = new List<MapObjectSerializedObjectAssociation>();

        foreach (AsteroidFieldControllerPersistance asteroidFieldControllerPersistance in gameSceneData.asteroidFields)
        {
            GameObject field = Spawner.Instance.SpawnAsteroidField(asteroidFieldControllerPersistance.mapObjectPersitance.id, new AsteroidFieldAsteroidSettings(), asteroidFieldControllerPersistance.mapObjectPersitance.localPosition, asteroidFieldControllerPersistance.size, false);

            asteroidFields.Add(new GameObjectSerializedObjectAssociation<AsteroidFieldControllerPersistance>(field, asteroidFieldControllerPersistance));

            mapObjects.Add(new MapObjectSerializedObjectAssociation(field.GetComponent<MapObject>(), asteroidFieldControllerPersistance.mapObjectPersitance));

            foreach (AsteroidControllerPersistance acp in asteroidFieldControllerPersistance.asteroids)
            {
                Vector3 position = acp.mapObjectPersitance.localPosition +  acp.mapObjectPersitance.localPosition;
                GameObject asteroid;
                if(acp.prefabIndex == -1)
                {
                    asteroid = Spawner.Instance.SpawnAsteroid(acp.mapObjectPersitance.id, field.GetComponent<AsteroidFieldController>(), acp.resourceType, acp.resourceQuantity, position, false);
                }
                else
                {
                    asteroid = Spawner.Instance.SpawnAsteroid(acp.prefabIndex,  acp.mapObjectPersitance.id, field.GetComponent<AsteroidFieldController>(), acp.resourceType, acp.resourceQuantity, position, false);
                }

                asteroids.Add(new GameObjectSerializedObjectAssociation<AsteroidControllerPersistance>(asteroid, acp));
                mapObjects.Add(new MapObjectSerializedObjectAssociation(asteroid.GetComponent<MapObject>(), acp.mapObjectPersitance));
            }
        }

        foreach (BulletControllerPersistance bcp in gameSceneData.bulletControllerPersistances)
        {
            GameObject bullet = Spawner.Instance.SpawnBullet(bcp.mapObjectPersitance.id, BulletFactory.getInstance().CreateBullet(bcp.bullet.bulletType).prefab, bcp.mapObjectPersitance.localPosition, bcp.mapObjectPersitance.localRotation);
            bullets.Add(new GameObjectSerializedObjectAssociation<BulletControllerPersistance>(bullet, bcp));
            mapObjects.Add(new MapObjectSerializedObjectAssociation(bullet.GetComponent<MapObject>(), bcp.mapObjectPersitance));
        }

        foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipControllerPersistance sp in playerPersistance.ships)
            {
                GameObject ship = Spawner.Instance.SpawnShip(sp.mapObjectPersitance.id, sp.shipType, playerPersistance.player, sp.mapObjectPersitance.localPosition, Quaternion.identity, false);
                ship.GetComponent<ShipController>().initialized = sp.initialized;
                ships.Add(new GameObjectSerializedObjectAssociation<ShipControllerPersistance>(ship, sp));
                mapObjects.Add(new MapObjectSerializedObjectAssociation(ship.GetComponent<MapObject>(), sp.mapObjectPersitance));
            }

            foreach (StationControllerPersistance scp in playerPersistance.stations)
            {
                GameObject station = Spawner.Instance.SpawnStation(scp.mapObjectPersitance.id, scp.stationType, playerPersistance.player, scp.mapObjectPersitance.localPosition, scp.mapObjectPersitance.localRotation, scp.constructionProgress, false);
                stations.Add(new GameObjectSerializedObjectAssociation<StationControllerPersistance>(station, scp));
                mapObjects.Add(new MapObjectSerializedObjectAssociation(station.GetComponent<MapObject>(), scp.mapObjectPersitance));
            }
        }

        foreach (GameObjectSerializedObjectAssociation<AsteroidFieldControllerPersistance> association in asteroidFields)
        {
            association.gameObject.GetComponent<AsteroidFieldController>().SetObject(association.serializedObject);
        }

        foreach (GameObjectSerializedObjectAssociation<AsteroidControllerPersistance> association in asteroids)
        {
            association.gameObject.GetComponent<AsteroidController>().SetObject(association.serializedObject);
        }

        foreach (GameObjectSerializedObjectAssociation<ShipControllerPersistance> association in ships)
        {
            ShipController shipController = association.gameObject.GetComponent<ShipController>();

            if (shipController.initialized)
            {
                shipController.SetObject(association.serializedObject);
            }
        }

        foreach (GameObjectSerializedObjectAssociation<StationControllerPersistance> association in stations)
        {
            association.gameObject.GetComponent<StationController>().SetObject(association.serializedObject);
        }

        foreach (GameObjectSerializedObjectAssociation<BulletControllerPersistance> association in bullets)
        {
            association.gameObject.GetComponent<BulletController>().SetObject(association.serializedObject);
        }

        PlayerDatabase.Instance.SetObject(gameSceneData.players);

        foreach (MapObjectSerializedObjectAssociation association in mapObjects)
        {
            association.mapObject.SetObject(association.serializedObject);

            association.mapObject.gameObject.SetActive(true);
        }

        Spawner.Instance.nextId = gameSceneData.nextMapObjectId;
        if(gameSceneData.shipConstructionManagerPersistance != null)
        {
            ShipConstructionManager.Instance.SetObject(gameSceneData.shipConstructionManagerPersistance);
        }

        MapObject[] trueMapObjects = GameObject.FindObjectsOfType<MapObject>();
        for (int i = 0; i < trueMapObjects.Length; i++)
        {
           FogOfWarUtility.SetRendering(false, trueMapObjects[i].gameObject);
        }
        
        
    }

    private void SetUpPlayerManager()
    {
        Player player = null;
        for (int j = 0; j < GameInitializer.Instance.gameSceneData.players.Count; j++)
        {
            if (GameInitializer.Instance.gameSceneData.players[j].player.PlayerType == PlayerType.Real)
            {
                player = GameInitializer.Instance.gameSceneData.players[j].player;
            }
        }

        if (player == null)
        {
            return;
        }
        else
        {
            GameObject selectionPanel = Instantiate(selectionPanelPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
            GameObject constructionSection = selectionPanel.GetComponentInChildren<ConstructionSection>().gameObject;

            /*MouseCommandsController mouseCommandsController = playerManager.GetComponent<MouseCommandsController>();
            mouseCommandsController.selectPanel = selectionPanel;
            mouseCommandsController.constructionSection = constructionSection;*/

            playerManager.GetComponent<MapObjecsRenderingController>().players = new Player[1] {player};
            playerManager.GetComponent<FogOfWarController>().playersVision = new Player[1] {player};

            playerManager.GetComponent<MapObjecsRenderingController>().enabled = true;
            playerManager.GetComponent<FogOfWarController>().enabled = true;

            playerManager.SetActive(true);
        }
    }

    private class GameObjectSerializedObjectAssociation<SerializedObjectType>
    {
        public GameObject gameObject;
        public SerializedObjectType serializedObject;

        public GameObjectSerializedObjectAssociation(GameObject gameObject, SerializedObjectType serializedObject)
        {
            this.gameObject = gameObject;
            this.serializedObject = serializedObject;
        }
    }

    private class MapObjectSerializedObjectAssociation
    {
        public MapObject mapObject;
        public MapObjectPersitance serializedObject;

        public MapObjectSerializedObjectAssociation(MapObject mapObject, MapObjectPersitance serializedObject)
        {
            this.mapObject = mapObject;
            this.serializedObject = serializedObject;
        }
    }

    /* private void CreateAsteroidFields()
     {
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
     }*/
}