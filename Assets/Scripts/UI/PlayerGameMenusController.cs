﻿using UnityEngine;

public class PlayerGameMenusController : MonoBehaviour
{
    public GameObject gameMainMenuObject;

    private GameMainMenu gameMainMenu;

    private void Start()
    {
        gameMainMenu = gameMainMenuObject.GetComponent<GameMainMenu>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMainMenu.Show(!gameMainMenuObject.activeSelf);
        }
    }
}