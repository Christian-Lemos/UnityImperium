using Imperium;
using Imperium.MapObjects;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
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

        /*foreach (PlayerPersistance playerPersistance in gameSceneData.players)
        {
            foreach (ShipControllerPersistance shipPersistence in playerPersistance.ships)
            {
                Spawner.Instance.SpawnShip(shipPersistence.shipType, playerPersistance.playerNumber, shipPersistence.mapObjectPersitance.localPosition, Quaternion.identity);
            }
        }

        CreateAsteroidFields();

        SetUpPlayerManager();*/

        LoadGameSceneData();
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

            foreach (AsteroidControllerPersistance asteroidControllerPersistance in asteroidFieldControllerPersistance.asteroids)
            {
                GameObject asteroid = Spawner.Instance.SpawnAsteroid(asteroidControllerPersistance.mapObjectPersitance.id, field.GetComponent<AsteroidFieldController>(), asteroidControllerPersistance.resourceType, asteroidControllerPersistance.resourceQuantity
                    , new Vector3(asteroidControllerPersistance.mapObjectPersitance.localPosition.x + asteroidControllerPersistance.mapObjectPersitance.localPosition.x, asteroidControllerPersistance.mapObjectPersitance.localPosition.y + asteroidControllerPersistance.mapObjectPersitance.localPosition.y,
                    asteroidControllerPersistance.mapObjectPersitance.localPosition.z + asteroidControllerPersistance.mapObjectPersitance.localPosition.z), false);
                asteroids.Add(new GameObjectSerializedObjectAssociation<AsteroidControllerPersistance>(asteroid, asteroidControllerPersistance));
                mapObjects.Add(new MapObjectSerializedObjectAssociation(asteroid.GetComponent<MapObject>(), asteroidControllerPersistance.mapObjectPersitance));
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
                GameObject ship = Spawner.Instance.SpawnShip(sp.mapObjectPersitance.id, sp.shipType, playerPersistance.playerNumber, sp.mapObjectPersitance.localPosition, Quaternion.identity);
                ship.GetComponent<ShipController>().initialized = sp.initialized;
                ships.Add(new GameObjectSerializedObjectAssociation<ShipControllerPersistance>(ship, sp));
                mapObjects.Add(new MapObjectSerializedObjectAssociation(ship.GetComponent<MapObject>(), sp.mapObjectPersitance));
            }

            foreach (StationControllerPersistance scp in playerPersistance.stations)
            {
                GameObject station = Spawner.Instance.SpawnStation(scp.mapObjectPersitance.id, scp.stationType, playerPersistance.playerNumber, scp.mapObjectPersitance.localPosition, scp.mapObjectPersitance.localRotation, scp.constructionProgress, false);
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
            MeshRenderer[] meshRenderers = trueMapObjects[i].GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < meshRenderers.Length; j++)
            {
                meshRenderers[j].enabled = false;
            }

            TrailRenderer[] trailRenderers = trueMapObjects[i].GetComponentsInChildren<TrailRenderer>();
            for (int j = 0; j < trailRenderers.Length; j++)
            {
                trailRenderers[j].enabled = false;
            }
        }
        
        SetUpPlayerManager();
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