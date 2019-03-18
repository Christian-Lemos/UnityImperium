using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button exitGameButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button newGameButton;

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