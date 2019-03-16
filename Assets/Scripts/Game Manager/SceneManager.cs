using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static SceneManager s_instance;
    public GameSceneData currentGameSceneData;
    private PersistantDataManager persistantDataManager;

    public static SceneManager Instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = GameObject.Instantiate((GameObject) Resources.Load("Scene Manager")).GetComponent<SceneManager>();
                s_instance.currentGameSceneData = GameSceneData.NewGameDefault();
            }
            return s_instance;
        }
        private set
        {
            s_instance = value;
        }
    }

    public void CreateNewGame(int playerCount)
    {
        GameSceneData data = GameSceneData.NewGameDefault();
        persistantDataManager.CreateGameSceneData(data);
        currentGameSceneData = data;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

    public void LoadGame(string gameName)
    {
        GameSceneData gameSceneData = persistantDataManager.GetGameData(gameName);
        currentGameSceneData = gameSceneData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        currentGameSceneData = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
    }

    public void UpdateCurrentSceneData()
    {
        BulletController[] bulletControllers = FindObjectsOfType<BulletController>();
        List<BulletControllerPersistance> bulletControllerPersistances = new List<BulletControllerPersistance>();

        foreach (BulletController bulletController in bulletControllers)
        {
            bulletControllerPersistances.Add(bulletController.Serialize());
        }

        AsteroidFieldController[] asteroidFieldControllers = FindObjectsOfType<AsteroidFieldController>();
        List<AsteroidFieldControllerPersistance> asteroidFieldPersistances = new List<AsteroidFieldControllerPersistance>();
        foreach (AsteroidFieldController asteroidFieldController in asteroidFieldControllers)
        {
            asteroidFieldPersistances.Add(asteroidFieldController.Serialize());
        }

        currentGameSceneData.players = PlayerDatabase.Instance.Serialize();
        currentGameSceneData.asteroidFields = asteroidFieldPersistances;
        currentGameSceneData.shipConstructionManagerPersistance = ShipConstructionManager.Instance.Serialize();

        currentGameSceneData.bulletControllerPersistances = bulletControllerPersistances;
        currentGameSceneData.nextMapObjectId = Spawner.Instance.nextId;
    }

    private void Awake()
    {
        SceneManager[] sceneManagers = GameObject.FindObjectsOfType<SceneManager>();
        if(sceneManagers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        persistantDataManager = PersistantDataManager.Instance;
    }
}