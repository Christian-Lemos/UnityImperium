using Imperium.Persistence;
using UnityEngine;
using UnityEngine.UI;

public class GameMainMenu : MonoBehaviour
{
    public Button exitGameButton;

    public Button loadGameButton;

    public Button saveGameButton;

    private void ExitGameHandler()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void LoadGameHandler()
    {
        SceneManager.Instance.LoadGame("New Game");
    }

    private void SaveGameHandler()
    {
        SceneManager.Instance.UpdateCurrentSceneData();
        PersistantDataManager.Instance.SaveGame(SceneManager.Instance.currentGameSceneData);
    }

    private void Start()
    {
        exitGameButton.onClick.AddListener(ExitGameHandler);
        loadGameButton.onClick.AddListener(LoadGameHandler);
        saveGameButton.onClick.AddListener(SaveGameHandler);
    }

    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }
}