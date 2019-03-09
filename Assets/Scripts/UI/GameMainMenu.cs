using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Imperium.Persistence;
using Imperium.Persistence.MapObjects;
public class GameMainMenu : MonoBehaviour {

	[SerializeField]
    private Button exitGameButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button saveGameButton;


    private void ExitGameHandler()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void LoadGameHandler()
    {
        
    }

    private void SaveGameHandler()
    {
        BulletController[] bulletControllers = FindObjectsOfType<BulletController>();
        List<BulletControllerPersistance> bulletControllerPersistances = new List<BulletControllerPersistance>();

        foreach(BulletController bulletController in bulletControllers)
        {
            bulletControllerPersistances.Add(bulletController.Serialize());
        }

        GameSceneData current = SceneManager.Instance.CurrentGameSceneData;
        current.players = PlayerDatabase.Instance.Serialize();
        current.bulletControllerPersistances = bulletControllerPersistances;
        PersistantDataManager.Instance.SaveGame(current);
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
