using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameSceneData currentGameSceneData;
    private PersistantDataManager persistantDataManager;
    public static SceneManager Instance { get; private set; }

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
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        persistantDataManager = PersistantDataManager.Instance;
    }
}