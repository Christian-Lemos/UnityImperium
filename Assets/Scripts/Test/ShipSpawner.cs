using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Imperium.Enum;
public class ShipSpawner : MonoBehaviour {


    public GameObject inputJogadorGO;
    private InputField inputJogador;

    public GameObject buttonGO;
    private Button button;

    private Spawner spawner;
    
    private Vector3 lastClickedPosition = new Vector3(0, 0, 0);

    private int mapLayer = 1 << (int)ObjectLayers.Map;

    private void Start()
    {
        button = buttonGO.GetComponent<Button>();
        inputJogador = inputJogadorGO.GetComponent<InputField>();
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();

        button.onClick.AddListener(SpawnShip);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mapLayer))
            {
                lastClickedPosition = hit.point;
            }
        }
        
    }

    private void SpawnShip()
    {
        int jogador;
        if(Int32.TryParse(inputJogador.text, out jogador))
        {
            spawner.SpawnShip(ShipType.Test, jogador, lastClickedPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Failed to convert " + inputJogador.text + " to integer");
        }
    }


}
