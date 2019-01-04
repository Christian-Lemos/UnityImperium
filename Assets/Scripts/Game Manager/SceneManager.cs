using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Imperium.Persistence;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance { get; private set; }

    private PersistantDataManager persistantDataManager;

    public GameSceneData CurrentGameSceneData;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        DontDestroyOnLoad(this.gameObject);
        persistantDataManager = PersistantDataManager.Instance;
	}

    public void CreateNewGame(int playerCount)
    {
        GameSceneData data = GameSceneData.NewGameDefault();
        persistantDataManager.CreateGameSceneData(data);
        this.CurrentGameSceneData = data;
        UnityEngine.SceneManagement.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }
    

   
}
