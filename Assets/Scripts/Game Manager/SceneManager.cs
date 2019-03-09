using Imperium.Persistence;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameSceneData CurrentGameSceneData;
    private PersistantDataManager persistantDataManager;
    public static SceneManager Instance { get; private set; }



    public void CreateNewGame(int playerCount)
    {
        GameSceneData data = GameSceneData.NewGameDefault();
        persistantDataManager.CreateGameSceneData(data);
        CurrentGameSceneData = data;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

    public void LoadGame(string gameName)
    {
        GameSceneData gameSceneData = persistantDataManager.GetGameData(gameName);
        CurrentGameSceneData = gameSceneData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        CurrentGameSceneData = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
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