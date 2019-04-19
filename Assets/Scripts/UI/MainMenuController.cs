using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Button exitGameButton;

    public Button loadGameButton;

    public Button newGameButton;

    private void ExitGameHandler()
    {
        Application.Quit();
    }

    private void NewGameHandler()
    {
        SceneManager.Instance.CreateNewGame(2);
    }
    private void LoadGameHandler()
    {
        SceneManager.Instance.LoadGame("New Game");
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(NewGameHandler);
        loadGameButton.onClick.AddListener(LoadGameHandler);
        exitGameButton.onClick.AddListener(ExitGameHandler);
    }
}