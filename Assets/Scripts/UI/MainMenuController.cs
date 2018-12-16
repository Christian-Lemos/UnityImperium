using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour {
    
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private Button exitGameButton;

	void Start () {
        this.newGameButton.onClick.AddListener(NewGameHandler);
        this.exitGameButton.onClick.AddListener(ExitGameHandler);
	}
	
	
	private void NewGameHandler()
    {
        SceneManager.Instance.CreateNewGame(2);
    }
    private void ExitGameHandler()
    {
        Application.Quit();
    }
}
